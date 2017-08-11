using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Timer = System.Timers.Timer;


namespace VlcDanmuPlayerWpf.Danmu
{
    public class DanmuBox 
    {
        #region attributes & ctor        
        private Regex reg = new Regex(@"p=""(?<time>\d+\.\d*)[^>]+\>(?<content>[^<]*)\</d\>", RegexOptions.IgnorePatternWhitespace);

        private Dictionary<int, string[]> _danmuDict;
        
        //private List<DanmuItem> danmuShowing;
        public ConcurrentBag<DanmuItem> DanmuShowing { get; private set; }

        private Random rand = new Random(Environment.TickCount);

        private Timer timer;

        public DanmuBox()
        {
            this.timer = new Timer()
            {
                Interval = 100,
            };
            this.timer.Elapsed += RollDanmu;
            this.DanmuShowing = new ConcurrentBag<DanmuItem>();
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

            this.timer.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playTime">count by 100ms</param>
        public void UpdateDanmnTexts(double playTime)
        {
            int time = (int)(playTime);
            if (!this._danmuDict.ContainsKey(time))
                return;
            if (this._danmuDict[time] == null ||
                this._danmuDict[time].Length == 0)
                return;

            foreach (var dm in this._danmuDict[time])
            {
                this.DanmuShowing.Add(new DanmuItem(1, dm, GetDrawLength(dm))
                {
                    LocationY = this.rand.NextDouble(),
                });
            }
        }

        //public TextBlock[] GetCurrentTextBlocks()
        //{
        //    return this.DanmuShowing.Select(d =>
        //    {
        //        var block = new TextBlock
        //        {
        //            Text = d.Content,
        //            Foreground = Brushes.Gray,
        //            FontSize = this.fontSize,
        //            FontFamily = this.fontFamily,
        //        };
        //        Canvas.SetLeft(block, d.LocationX);
        //        Canvas.SetTop(block, d.LocationY);
        //        return block;
        //    }).ToArray();
        //}

        private void RollDanmu(object obj, ElapsedEventArgs e)
        {
            this.DanmuShowing = new ConcurrentBag<DanmuItem>(this.DanmuShowing.Select(d =>
                {
                    d.LocationX -= 0.03;
                    return d;
                })
                .Where(d => d.LocationX + d.Length > 0));
        }

        private Typeface typeFace;
        private double fontSize;
        private FontFamily fontFamily;
        public void SetTextFormatPara(string fontFamilyName, double fontsize)
        {
            FontStyle style = new FontStyle();
            this.fontFamily = new FontFamily(fontFamilyName);
            FontStretch stretch = new FontStretch();
            this.typeFace = new Typeface(new FontFamily(fontFamilyName), 
                style, 
                new FontWeight(),
                stretch);
            this.fontSize = fontsize;
        }

        private double GetDrawLength(string str)
        {
            return MeasureString(str);
        }

        private double MeasureString(string str)
        {
            var formattedText = new FormattedText(
                str,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight, this.typeFace, this.fontSize,
                Brushes.Black);
            return new Size(formattedText.Width, formattedText.Height).Width;
        }
    }
}
