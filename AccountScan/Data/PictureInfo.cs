using System.Drawing;
using System.IO;

namespace AccountScan.Data
{
    public class PictureInfo
    {
        #region Attributes
        private string PageName { get; set; }

        public byte[] Image { get; set; }
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
