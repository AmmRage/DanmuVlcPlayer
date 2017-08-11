using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using VlcDanmuPlayerWpf.Danmu;

namespace VlcDanmuPlayerWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DanmuBox danmu;

        private string fontName;

        private double fontSize;

        private double width;
        private double height;

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();

            this.dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            this.dispatcherTimer.Interval = new TimeSpan(0, 0, 0,0, 100);

            this.danmu = new DanmuBox();
            this.fontName = ("Arial");
            this.fontSize = 14;
            this.Player.SizeChanged += Player_SizeChanged;
            this.width = this.Player.ActualWidth;
            this.height = this.Player.ActualHeight;

        }

        void Player_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.width = this.Player.ActualWidth;
            this.height = this.Player.ActualHeight;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.CanvasDanmu.Children.Clear();
            foreach (var d in this.danmu.DanmuShowing)
            {
                var block = new TextBlock
                {
                    Text = d.Content,
                    Foreground = Brushes.Gray,
                    FontSize = this.fontSize,
                };
                Canvas.SetLeft(block, d.LocationX * this.width);
                Canvas.SetTop(block, d.LocationY * this.height);
                this.CanvasDanmu.Children.Add(block);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Player.LoadMedia(@"");

            this.Player.TimeChanged += Player_TimeChanged;
            this.Player.Play();
            //DrawCanvas();
            this.danmu.SetTextFormatPara(this.fontName, this.fontSize);
            this.danmu.LoadDanmuFile(@"");
            this.dispatcherTimer.Start();
        }

        private void Player_TimeChanged(object sender, EventArgs e)
        {
            this.danmu.UpdateDanmnTexts(this.Player.Time.TotalMilliseconds / 100);
        }

        private void DrawCanvas()
        {         
            this.CanvasDanmu.Children.Clear();
            TextBlock textBlock = new TextBlock
            {
                Text = "asdfalsdjfk",
                Foreground = Brushes.Cyan,
                FontSize = 32,
            };
            Canvas.SetLeft(textBlock, 100);
            Canvas.SetTop(textBlock, 100);
            this.CanvasDanmu.Children.Add(textBlock);
        }

        protected override void OnClosed(EventArgs e)
        {
            this.Player.Stop();
            while (this.Player.State != Meta.Vlc.Interop.Media.MediaState.Stopped)
            {
                Thread.Sleep(10);
            }
            this.Player.Dispose();
            base.OnClosed(e);
        }
    }
}

