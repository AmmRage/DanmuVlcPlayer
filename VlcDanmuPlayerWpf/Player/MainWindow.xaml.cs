﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Win32;
using VlcDanmuPlayerWpf.Danmu;
using VlcDanmuPlayerWpf.Properties;
using Path = System.IO.Path;

namespace VlcDanmuPlayerWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region attributes & ctor

        private DanmuBox danmu;

        private string fontName;

        private double fontSize;

        private double width;

        private double height;

        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            
            this.Left = Settings.Default.Location.X;
            this.Top = Settings.Default.Location.Y;

            this.dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            this.dispatcherTimer.Interval = new TimeSpan(0, 0, 0,0, 100);

            this.danmu = new DanmuBox();
            this.fontName = ("Arial");
            this.fontSize = 14;          

            this.Player.SizeChanged += Player_SizeChanged;
            this.width = this.Player.ActualWidth;
            this.height = this.Player.ActualHeight;

            this.danmu.SetTextFormatPara(this.fontName, this.fontSize);

            this.Player.TimeChanged += Player_TimeChanged;
        }
        #endregion 

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

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Player_SizeChanged(object sender, SizeChangedEventArgs e)
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
                    Foreground = Brushes.LightGray,
                    FontSize = this.fontSize,
                };
                Canvas.SetLeft(block, d.LocationX * this.width);
                Canvas.SetTop(block, d.LocationY * this.height);
                this.CanvasDanmu.Children.Add(block);
            }
        }

        private void Player_TimeChanged(object sender, EventArgs e)
        {
            this.danmu.UpdateDanmnTexts(this.Player.Time.TotalMilliseconds / 100);
        }

        private void LabelTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void VlcPlayerMainWindow_LocationChanged(object sender, EventArgs e)
        {          
            Settings.Default.Location = new System.Drawing.Point((int) this.Left, (int) this.Top);
            Settings.Default.Save();
        }
    }
}
