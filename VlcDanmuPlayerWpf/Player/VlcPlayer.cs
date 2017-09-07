using System;
using System.IO;
using Meta.Vlc;
using Meta.Vlc.Interop.Media;
using Microsoft.Win32;

namespace VlcDanmuPlayerWpf
{
    public partial class MainWindow
    {
        private void InitPlayerEventHandlers()
        {
            this.Player.TimeChanged += Player_TimeChanged;
            this.Player.StateChanged += Player_StateChanged;
        }

        private void Play(string file)
        {
            if (this.Player.VlcMediaPlayer.IsPlaying)
                return;
            this.Player.LoadMedia(file);
            this.Player.Play();
            var damnfile = Path.Combine(
                Path.GetDirectoryName(file),
                Path.GetFileNameWithoutExtension(file) + ".cmt.xml");
            this.danmu.LoadDanmuFile(damnfile);
            this.dispatcherTimer.Start();
        }

        private void Stop()
        {
            if (this.Player.VlcMediaPlayer.IsPlaying)
            {
                this.Player.VlcMediaPlayer.Stop();
                this.danmu.Reset();
            }
        }

        void Player_StateChanged(object sender, ObjectEventArgs<MediaState> e)
        {
            if (e.Value == MediaState.Playing)
            {
                this.MediaLength = (int)this.Player.VlcMediaPlayer.Length.TotalSeconds;
            }
        }

        private void Player_TimeChanged(object sender, EventArgs e)
        {
            this.danmu.UpdateDanmnTexts(this.Player.Time.TotalMilliseconds / 100);
            PlayProgress = (int)this.Player.Time.TotalSeconds;
        }
    }
}
