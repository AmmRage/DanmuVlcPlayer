using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vlc.DotNet.Core;
using Vlc.DotNet.Forms;
using System.Timers;
using Timer = System.Timers.Timer;


namespace VlcDemo
{
    public partial class DanmuBox 
    {
        #region attributes & ctor        
        private Regex reg = new Regex(@"p=""(?<time>\d+\.\d*)[^>]+\>(?<content>[^<]*)\</d\>", RegexOptions.IgnorePatternWhitespace);

        private Dictionary<int, string[]> _danmuDict;

        private List<DanmuItem> danmuShowing;

        private Graphics g;

        private Font font;

        private SolidBrush brush;

        private Random rand = new Random(Environment.TickCount);

        private Timer timer;

        public DanmuBox()
        {
            this.timer = new Timer()
            {
                Interval = 100,
            };
            this.timer.Elapsed += RollDanmu;
            this.danmuShowing = new List<DanmuItem>();
        }
        #endregion

        public void LoadDanmuFile(string danmufile)
        {
            this._danmuDict = File.ReadLines(danmufile).Select(l =>
                {
                    var m = this.reg.Match(l);
                    if (string.IsNullOrWhiteSpace(m.Groups["time"].Value) ||
                        string.IsNullOrWhiteSpace(m.Groups["content"].Value))
                        return null;
                    return new Tuple<int, string>(
                        (int)(double.Parse(m.Groups["time"].Value) * 10),
                        m.Groups["content"].Value);
                })
                .Where(t => t != null)
                .GroupBy(t => t.Item1, t => t.Item2)
                .ToDictionary(g => g.Key, g => g.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playTime">count by 100ms</param>
        public void UpdateDanmnTexts(int playTime)
        {
            int time = (int)(playTime);
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

        protected void OnPaint(PaintEventArgs e)
        {
            //var width = e.Graphics.VisibleClipBounds.Width;
            //var height = e.Graphics.VisibleClipBounds.Height;
            //for (var i = 0; i < this.danmuShowing.Count; i++)
            //{
            //    e.Graphics.DrawString(this.danmuShowing[i].Content,
            //        this.font,
            //        this.brush,
            //        (float)(this.danmuShowing[i].LocationX * width),
            //        (float)(this.danmuShowing[i].LocationY * height));
            //}
        }

        private void RollDanmu(object obj, ElapsedEventArgs e)
        {
            this.danmuShowing = this.danmuShowing.Select(d =>
                {
                    d.LocationX -= 0.03;
                    return d;
                })
                .Where(d => d.LocationX + d.Length > 0)
                .ToList();
        }

        private double GetDrawLength(string str)
        {
            return this.g.MeasureString(str, this.font).Width;
        }
    }
}
