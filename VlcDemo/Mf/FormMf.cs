using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using VlcDanmuPlayerForms.Properties;

namespace VlcDemo
{
    public partial class FormMf : Form
    {
        private MfPlayer mfplayer;
        public FormMf()
        {
            InitializeComponent();
            this.Opacity = 0.5;

            //this.Controls.Clear();
        }

        public void AddVlcPlay()
        {
            this.mfplayer = new MfPlayer();
            this.mfplayer.Attach(this.panelDisplay);
        }

        private void FormMf_Shown(object sender, EventArgs e)
        {
            AddVlcPlay();
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
            if (this.listBoxList.Items.Count == 0)
                return;
            //if (this.myVlcControl.IsPlaying)
            //    this.myVlcControl.Stop();
            var file = (this.listBoxList.SelectedItem as MediaInfo).FileFullname;
            var extension = Path.GetExtension(file);
            try
            {
                //if (File.Exists(file.Replace(extension, ".cmt.xml")))
                //    this.damnu.LoadDanmuFile(file.Replace(extension, ".cmt.xml"));
                this.mfplayer.Play(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            this.openFileDialogOpen.InitialDirectory = Settings.Default.HistotyOpenLocation;//Environment.GetFolderPath(Environment.SpecialFolder.Desktop),

            var result = this.openFileDialogOpen.ShowDialog();
            if (result != DialogResult.OK)
                return;
            
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
            this.trackBarSeek.Value = 0;
            Play();
            Debug.WriteLine("play started");
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            if (this.mfplayer.IsPlaying)
            {
                this.mfplayer.Pause();
                (sender as Button).Text = "Resume";
            }
            else
            {
                this.mfplayer.Play();
                (sender as Button).Text = "Pause";
            }
        }

        private void trackBarSeek_Scroll(object sender, EventArgs e)
        {
            this.mfplayer.Position = ((float)(sender as TrackBar).Value) / 100;
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

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.mfplayer.Stop();
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                this.mfplayer.Stop();
                //while (this.mfplayer.State != MediaStates.Stopped)
                //{
                //    Thread.Sleep(200);
                //}
            }
            catch(Exception ex)
            {

            }
            base.OnClosed(e);
        }
    }
}
