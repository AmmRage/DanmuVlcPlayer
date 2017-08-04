using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VlcDemo
{
    public partial class ControlTest : Form
    {
        public ControlTest()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.DrawString("sdfa", SystemFonts.DefaultFont, new SolidBrush(Color.Blue), 0, 0);
            //base.OnPaint(e);
        }

        private void ControlTest_Shown(object sender, EventArgs e)
        {
            this.danmuBox1.LoadDanmuFile(
                @"C:\Users\ZhiYong\Documents\Projects\py\you-get\「木JJ出品」Billboard 美国单曲周榜第32期 2017.cmt.xml");
        }

        private int time = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            this.timerRoll.Enabled = true;
        }

        private void timerRoll_Tick(object sender, EventArgs e)
        {
            var vlce = new Vlc.DotNet.Core.VlcMediaPlayerTimeChangedEventArgs(time);
            time += 100;
            this.danmuBox1.RollDanmu(null, vlce);
        }
    }
}
