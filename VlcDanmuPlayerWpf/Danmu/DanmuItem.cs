namespace VlcDanmuPlayerWpf.Danmu
{
    public class DanmuItem
    {
        /// <summary>
        /// relative
        /// </summary>
        public double LocationX;
        /// <summary>
        /// relative
        /// </summary>
        public double LocationY;
        public string Content;
        /// <summary>
        /// relative
        /// </summary>
        public double Length;

        public DanmuItem(double x, string content, double len)
        {
            this.LocationX = x;
            this.LocationY = 0;
            this.Content = content;
            this.Length = len;
        }
    }
}