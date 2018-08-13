using AccountScan.Data;
using AccountScan.Util;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AccountScan
{
    public partial class MainForm : Form
    {
        #region Method
        private void Detect()
        {
            var info = (PictureInfo)PictureListBox.SelectedItem;

            if (info == null)
            {
                return;
            }
            using (var img = info.Bitmap)
            using (var mat = new Image<Bgr, byte>(img).Mat)
            {
                var rect = new Rectangle(0, 0, img.Width / 4, img.Height / 2);

                using (var clip = new Mat(mat, rect))
                {
                    //CvInvoke.Imshow("source", clip);
                    OcrUtils.Recognize(clip, "jpn");
                }
            }
        }
        #endregion

        #region PictureListBox
        private void AddPage(string filename)
        {
            var imgconv = new ImageConverter();
            byte[] bytes;

            using (var img = Image.FromFile(filename))
            {
                bytes = (byte[])imgconv.ConvertTo(img, typeof(byte[]));
            }
            PictureListBox.Items.Add(new PictureInfo(filename)
            {
                Image = bytes,
            });
        }

        private void SelectLatestItem()
        {
            var count = PictureListBox.Items.Count;

            if (count == 0)
            {
                return;
            }
            var page = (PictureInfo)PictureListBox.Items[count - 1];

            PictureListBox.SelectedItem = page;
        }

        private void PictureListBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }
            e.Effect = DragDropEffects.Copy;
        }

        private void PictureListBox_DragDrop(object sender, DragEventArgs e)
        {
            var nameList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            var jpegList = nameList.Where(str => str.EndsWith(".jpg") || str.EndsWith(".jpeg"))
                .ToList();

            jpegList.ForEach(jpg => AddPage(jpg));
            SelectLatestItem();
        }

        private void PictureListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var info = (PictureInfo)PictureListBox.SelectedItem;

            AccountPictureBox.Image?.Dispose();
            AccountPictureBox.Image = info.Bitmap;
        }
        #endregion

        #region Begin/End
        private void Initialize()
        {
            DetectButton.Click += (sender, e) => Detect();
            ExitButton.Click += (sender, e) => Close();
        }

        public MainForm()
        {
            InitializeComponent();
            Load += (sender, e) => Initialize();
        }
        #endregion
    }
}
