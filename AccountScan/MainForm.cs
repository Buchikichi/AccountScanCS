using AccountScan.Data;
using AccountScan.Util;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace AccountScan
{
    public partial class MainForm : Form
    {
        #region Method
        private void SetStatusLabel(string text)
        {
            StatusLabel.Text = text;
            StatusBar.Refresh();
        }

        private void Detect()
        {
            var info = (PictureInfo)PictureListBox.SelectedItem;

            if (info == null)
            {
                return;
            }
            var detector = new AccountDetector();

            info.Region = detector.DetectTarget(info, (y, text) => {
                SetStatusLabel($"y={y}:[{text}]");
            });
            SetPicture(info);
        }
        #endregion

        #region AccountPictureBox
        private void SetPicture(PictureInfo info)
        {
            var bmp = info.Bitmap;
            var region = info.Region;

            if (!region.IsEmpty)
            {
                var clipRect = new Rectangle(0, region.Bottom, bmp.Width, CLIP_HEIGHT);

                using (var g = Graphics.FromImage(bmp))
                using (var red = new Pen(Color.Red, 5))
                using (var green = new Pen(Color.Green, 5) { DashStyle = DashStyle.Dash })
                {
                    g.DrawRectangle(red, info.Region);
                    g.DrawRectangle(green, clipRect);
                }
            }
            AccountPictureBox.Image?.Dispose();
            AccountPictureBox.Image = bmp;
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

            SetPicture(info);
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

        #region Member
        private const int CLIP_HEIGHT = 450;
        #endregion
    }
}
