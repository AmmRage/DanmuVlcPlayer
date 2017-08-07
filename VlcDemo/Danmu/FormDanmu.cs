using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Vlc.DotNet.Core;

namespace VlcDemo.Danmu
{
    public partial class FormDanmu : Form
    {
        private Regex reg = new Regex(@"p=""(?<time>\d+\.\d*)[^>]+\>(?<content>[^<]*)\</d\>",
            RegexOptions.IgnorePatternWhitespace);

        private Dictionary<int, string[]> _danmuDict;

        private List<DanmuItem> danmuShowing;

        private CancellationTokenSource cancelSource = new CancellationTokenSource();

        private Graphics g;

        private Font font;

        private SolidBrush brush;

        private Random rand = new Random(Environment.TickCount);

        public FormDanmu()
        {
            InitializeComponent();
            this.font = new Font("consolas", 12);
            this.brush = new SolidBrush(Color.FromArgb(125, Color.Gray));
            this.danmuShowing = new List<DanmuItem>();
            this.g = CreateGraphics();
        }

        public FormDanmu(Point location, Size size)
            : this()
        {
            this.Location = location;
            this.Size = size;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (this.DesignMode)
                return;
            return;
        }

        public void LoadDanmuFile(string danmufile)
        {
            this._danmuDict = File.ReadLines(danmufile)
                .Select(l =>
                {
                    var m = this.reg.Match(l);
                    if (string.IsNullOrWhiteSpace(m.Groups["time"].Value) ||
                        string.IsNullOrWhiteSpace(m.Groups["content"].Value))
                        return null;
                    return new Tuple<int, string>(
                        (int) (double.Parse(m.Groups["time"].Value) * 10),
                        m.Groups["content"].Value);
                })
                .Where(t => t != null)
                .GroupBy(t => t.Item1, t => t.Item2)
                .ToDictionary(g => g.Key, g => g.ToArray());
            this.timerRoll.Enabled = true;
        }

        public void RollDanmu(object sender, VlcMediaPlayerTimeChangedEventArgs e)
        {
            int time = (int) (e.NewTime / 100);
            if (!this._danmuDict.ContainsKey(time))
                return;
            if (this._danmuDict[time] == null ||
                this._danmuDict[time].Length == 0)
                return;

            foreach (var dm in this._danmuDict[time])
            {
                this.danmuShowing.Add(new DanmuItem(1, dm, GetDrawLength(dm))
                {
                    LocationY = this.rand.NextDouble(),
                });
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);          
            e.Graphics.Clear(Color.Transparent);
            var width = e.Graphics.VisibleClipBounds.Width;
            var height = e.Graphics.VisibleClipBounds.Height;
            for (var i = 0; i < this.danmuShowing.Count; i++)
            {
                e.Graphics.DrawString(this.danmuShowing[i].Content,
                    this.font,
                    this.brush,
                    (float) (this.danmuShowing[i].LocationX * width),
                    (float) (this.danmuShowing[i].LocationY * height));
            }
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            this.danmuShowing = this.danmuShowing.Select(d =>
                {
                    d.LocationX -= 0.03;
                    return d;
                })
                .Where(d => d.LocationX + d.Length > 0)
                .ToList();
            Invalidate();
            var vlce = new VlcMediaPlayerTimeChangedEventArgs(time);
            time += 100;
            RollDanmu(null, vlce);
        }

        private double GetDrawLength(string str)
        {
            return this.g.MeasureString(str, this.font).Width;
        }

        private int time = 0;
        public void SetLocation(Point location)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<Point>(SetLocation), location);
            }
            else
                this.Location = location;
        }

        public void SetSize(Size size)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<Size>(SetSize), size);
            }
            else
                this.Size = size;
        }

        public void SetClose()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(SetClose));
            }
            else
            {
                Debug.WriteLine("danmu form close");
                this.Close();
            }
        }
    }
}
