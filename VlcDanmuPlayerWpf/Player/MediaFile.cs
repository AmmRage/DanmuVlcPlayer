using System.IO;

namespace VlcDanmuPlayerWpf
{
    public class MediaFile
    {
        public string DisplayName;
        public string FileFullname;

        public MediaFile(string file)
        {
            this.FileFullname = file;
            this.DisplayName = Path.GetFileName(file);
        }

        public override string ToString()
        {            
            return this.DisplayName;
        }
    }
}