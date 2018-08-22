using System.Drawing;
using System.IO;

namespace AccountScan.Data
{
    public class PictureInfo
    {
        #region Attributes
        // A4 300dpi 3508×2480
        private const int A4Width = (int)(2480 * .99);
        private const int A4Height = (int)(3508 * .99);
        private string PageName { get; set; }

        public byte[] Image { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsA4
        {
            get
            {
                if (A4Width < Width && A4Height < Height)
                {
                    return true;
                }
                if (A4Width < Height && A4Height < Width)
                {
                    return true;
                }
                return false;
            }
        }
        public Rectangle Region { get; set; }
        public Bitmap Bitmap
        {
            get
            {
                var imgconv = new ImageConverter();
                var bmp = (Bitmap)imgconv.ConvertFrom(Image);

                return bmp;
            }
        }
        #endregion

        #region Begin/End
        public override string ToString() => PageName;

        public PictureInfo(string name)
        {
            if (File.Exists(name))
            {
                PageName = Path.GetFileName(name);
                return;
            }
            PageName = name;
        }
        #endregion
    }
}
