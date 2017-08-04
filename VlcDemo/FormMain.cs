using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Vlc.DotNet.Core.Interops.Signatures;
using Vlc.DotNet.Forms;
using VlcDemo.Properties;

namespace VlcDemo
{
    public partial class FormMain : Form
    {
        private VlcControl myVlcControl;
        private DanmuBox danmuBox1;

        public FormMain()
        {
            InitializeComponent();
            AddVlcPlay();
            this.Opacity = 0.5;
        }

        public void AddVlcPlay()
        {
            this.myVlcControl = new VlcControl();

            ((ISupportInitialize)(this.myVlcControl)).BeginInit();

            //this.myVlcControl.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom| AnchorStyles.Left)| AnchorStyles.Right;
            this.myVlcControl.Dock = DockStyle.Fill;
            this.myVlcControl.BackColor = SystemColors.ButtonShadow;
            //this.myVlcControl.Location = new Point(12, 12);
            this.myVlcControl.Name = "myVlcControl";
            //this.myVlcControl.Size = new Size(564, 338);
            this.myVlcControl.TabIndex = 0;
            this.myVlcControl.Text = "vlcRincewindControl1";
            this.myVlcControl.VlcLibDirectory = new DirectoryInfo(@"C:\Users\ZhiYong\Documents\Projects\Private\Practice\VlcDemo\bin\lib\x86");

            this.myVlcControl.EndReached += MyVlcControlEndReached;
            this.myVlcControl.Playing += MyVlcControlPlaying;
            this.myVlcControl.Paused += MyVlcControlPaused;
            this.myVlcControl.PositionChanged += MyVlcControlPositionChanged;
            this.myVlcControl.EncounteredError += MyVlcControlEncounteredError;
            this.myVlcControl.VlcLibDirectoryNeeded += MyVlcControlMyVlcControlLibDirectoryNeeded;


            //this.myVlcControl.TimeChanged += (o, e) =>
            //{
            //    Debug.WriteLine(e.NewTime);
            //};
            this.panelDisplay.Controls.Add(this.myVlcControl);
            ((ISupportInitialize)(this.myVlcControl)).EndInit();

            this.myVlcControl.BringToFront();

            danmuBox1 = new DanmuBox();

            danmuBox1.Location = this.myVlcControl.Location;
            danmuBox1.Name = "danmuBox1";
            danmuBox1.Size = this.myVlcControl.Size;
            danmuBox1.TabIndex = 0;
            this.panelDisplay.Controls.Add(danmuBox1);
            danmuBox1.BringToFront();

            this.myVlcControl.TimeChanged += danmuBox1.RollDanmu;
        }

        private void MyVlcControlMyVlcControlLibDirectoryNeeded(object sender, VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            if (currentDirectory == null)
                return;
            if (AssemblyName.GetAssemblyName(currentAssembly.Location).ProcessorArchitecture == ProcessorArchitecture.X86)
                e.VlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, @"..\..\..\lib\x86\"));
            else
                e.VlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, @"..\..\..\lib\x64\"));

            if (e.VlcLibDirectory.Exists) return;

            var folderBrowserDialog = new FolderBrowserDialog
            {
                Description = "Select Vlc libraries folder.",
                RootFolder = Environment.SpecialFolder.Desktop,
                ShowNewFolderButton = true
            };
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                e.VlcLibDirectory = new DirectoryInfo(folderBrowserDialog.SelectedPath);
            }
        }

        private void MyVlcControlEncounteredError(object sender, Vlc.DotNet.Core.VlcMediaPlayerEncounteredErrorEventArgs e)
        {
            Debug.WriteLine(e);
            //MessageBox.Show(e.ToString());
        }

        private void MyVlcControlPositionChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerPositionChangedEventArgs e)
        {
            //Debug.WriteLine(e.NewPosition);
            ShowPlayProgress(e.NewPosition);
        }

        private void MyVlcControlPaused(object sender, Vlc.DotNet.Core.VlcMediaPlayerPausedEventArgs e)
        {
            Debug.WriteLine("paused");
        }

        private void MyVlcControlPlaying(object sender, Vlc.DotNet.Core.VlcMediaPlayerPlayingEventArgs e)
        {
            Debug.WriteLine("playing");
        }

        private void MyVlcControlEndReached(object sender, Vlc.DotNet.Core.VlcMediaPlayerEndReachedEventArgs e)
        {
            Debug.WriteLine("ended");
            //SetPlayListIndex(1);
        }

        private void SetPlayListIndex(int movement)
        {
            if (this.listBoxList.InvokeRequired)
            {
                this.listBoxList.Invoke(new Action<int>(SetPlayListIndex), movement);
            }
            else
            {
                var index = this.listBoxList.SelectedIndex + movement;
                if (index < 0 || index >= this.listBoxList.Items.Count)
                    return;
                this.listBoxList.SelectedIndex = index;
            }
        }

        private void Play()
        {
            if (this.myVlcControl.IsPlaying)
                this.myVlcControl.Stop();
            var file = (this.listBoxList.SelectedItem as MediaInfo).FileFullname;
            try
            {
                if(File.Exists(file.Replace(".mp4", ".cmt.xml")))
                    danmuBox1.LoadDanmuFile(file.Replace(".mp4", ".cmt.xml"));
                this.myVlcControl.Play(new FileInfo(file));
            }
            catch (Exception ex)
            {
            }
            
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            this.openFileDialogOpen.InitialDirectory = Settings.Default.HistotyOpenLocation;//Environment.GetFolderPath(Environment.SpecialFolder.Desktop),

            var result = this.openFileDialogOpen.ShowDialog();
            if (result != DialogResult.OK)
                return;
            //Settings.Default.HistotyOpenLocation = openFileDialogOpen.
            
            if (this.openFileDialogOpen.FileNames == null)
                return;
            if (this.openFileDialogOpen.FileNames.Length > 0)
            {
                this.listBoxList.Items.AddRange(this.openFileDialogOpen.FileNames.Select(f => new MediaInfo(f)).ToArray());
            }
            this.listBoxList.SelectedIndex = 0;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {

        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            Play();
        }

        private void listBoxList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine(this.myVlcControl.InvokeRequired);
            //Debug.WriteLine(this.listBoxList.SelectedIndex);
            //this.myVlcControl.Invoke(new Action(() =>
            //{
            //    this.myVlcControl.Stop();
            //}));

            this.trackBarSeek.Value = 0;
            Play();
            Debug.WriteLine("play started");
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            if (this.myVlcControl.IsPlaying)
            {
                this.myVlcControl.Pause();
                (sender as Button).Text = "Resume";
            }
            else
            {
                this.myVlcControl.Play();
                (sender as Button).Text = "Pause";
            }
        }

        private void trackBarSeek_Scroll(object sender, EventArgs e)
        {
            this.myVlcControl.Position = ((float) (sender as TrackBar).Value)/100;
            //Debug.WriteLine("Trackbar changed");
        }

        private void ShowPlayProgress(float position)
        {
            if (this.trackBarSeek.InvokeRequired)
            {
                this.trackBarSeek.Invoke(new Action<float>(ShowPlayProgress), position);
            }
            else
            {
                var value = (int)( position*100);
                this.trackBarSeek.Value = value;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                this.myVlcControl.Stop();
                while (this.myVlcControl.State != MediaStates.Stopped)
                {
                    Thread.Sleep(200);
                }
                //this.myVlcControl.Dispose();
            }
            catch
            {
            }
            base.OnClosed(e);
            
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.myVlcControl.Stop();
        }
    }
}
