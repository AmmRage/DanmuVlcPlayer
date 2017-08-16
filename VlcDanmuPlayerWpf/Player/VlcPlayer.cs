using System.IO;
using Microsoft.Win32;

namespace VlcDanmuPlayerWpf
{
    public partial class MainWindow
    {
        private void Play()
        {
            if (this.Player.VlcMediaPlayer.IsPlaying)
                return;
            var ofd = new OpenFileDialog()
            {
                RestoreDirectory = true,
                Title = "Open a file",
                Multiselect = false,
            };
            if (ofd.ShowDialog() != true) return;

            this.Player.LoadMedia(ofd.FileName);
            this.Player.Play();
            var damnfile = Path.Combine(
                Path.GetDirectoryName(ofd.FileName),
                Path.GetFileNameWithoutExtension(ofd.FileName) + ".cmt.xml");
            this.danmu.LoadDanmuFile(damnfile);
            this.dispatcherTimer.Start();
        }
    }
}