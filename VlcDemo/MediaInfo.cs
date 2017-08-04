using System.IO;

namespace VlcDemo
{
    public class MediaInfo
    {
        public string DisplayName { get; private set; }
        public string FileFullname { get; private set; }

        public MediaInfo(string fullname)
        {
            this.FileFullname = fullname;
            this.DisplayName = Path.GetFileName(fullname);
        }

        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}