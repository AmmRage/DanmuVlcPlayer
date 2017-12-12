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

        private bool allowSeek = false;

        private void Seek(double postion)
        {
            if (allowSeek)
                this.Player.Position = (float) (postion / this.Player.VlcMediaPlayer.Length.TotalSeconds);
        }

        private string totalLengthString;
        private TimeSpan totalLength;

        private TimeSpan current;

        private void Player_StateChanged(object sender, ObjectEventArgs<MediaState> e)
        {
            if (e.Value == MediaState.Playing)
            {
                totalLength = this.Player.VlcMediaPlayer.Length;
                totalLengthString = string.Format("{0}:{1}:{2}",
                    totalLength.Hours == 0 ? "" : totalLength.Hours.ToString(),
                    totalLength.Minutes == 0 ? "" : totalLength.Minutes.ToString(),
                    totalLength.Seconds == 0 ? "" : totalLength.Seconds.ToString());
                this.MediaLength = (int) totalLength.TotalSeconds;
            }
        }

        private void Player_TimeChanged(object sender, EventArgs e)
        {
            if (allowSeek)
                return;
            this.danmu.UpdateDanmnTexts(this.Player.Time.TotalMilliseconds / 100);
            current = this.Player.Time;
            this.PlayProgress = (int)current.TotalSeconds;
            this.Dispatcher.Invoke(new Action(() =>
                {
                    this.LabelPlayTime.Content =
                        string.Format("{0}:{1}:{2}",
                            current.Hours == 0 ? "" : current.Hours.ToString(),
                            current.Minutes == 0 ? "" : current.Minutes.ToString(),
                            current.Seconds == 0 ? "" : current.Seconds.ToString()) +
                        " / " +
                        totalLengthString;
                }
            ));
        }
    }
}
