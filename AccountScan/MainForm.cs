using AccountScan.Data;
using AccountScan.Util;
using System;
using System.Data;
using System.Diagnostics;
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
            var sw = new Stopwatch();
            var detector = new AccountDetector();

            sw.Start();
            SetStatusLabel("検出しています...");
            info.Region = detector.DetectTarget(info);
            sw.Stop();
            if (info.Region.IsEmpty)
            {
                SetStatusLabel("検出失敗。");
                return;
            }
            var ts = sw.Elapsed;
            SetStatusLabel($"検出しました。[{ts.TotalSeconds}]");
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
                var width = bmp.Width - CLIP_PADDING * 2;
                var clipRect = new Rectangle(CLIP_PADDING, region.Top, width, CLIP_HEIGHT);

                using (var g = Graphics.FromImage(bmp))
                using (var red = new Pen(Color.Red, 5))
                using (var green = new Pen(Color.Green, 5) { DashStyle = DashStyle.Dash })
                {
                    g.DrawRectangle(red, info.Region);
                    g.DrawRectangle(green, clipRect);
                }
            }
            else
            {
                var x = AccountDetector.DETECT_LEFT_MARGIN + (int)(info.Width * AccountDetector.DETECT_HORIZONTAL_RATIO);
                var y = (int)(info.Height * AccountDetector.DETECT_VERTICAL_RATIO);

                using (var g = Graphics.FromImage(bmp))
                using (var blue = new Pen(Color.Blue, 4) { DashStyle = DashStyle.Dash })
                {
                    g.DrawRectangle(blue, new Rectangle(0, 0, x, y));
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

            using (var img = Image.FromFile(filename))
            {
                PictureListBox.Items.Add(new PictureInfo(filename)
                {
                    Image = (byte[])imgconv.ConvertTo(img, typeof(byte[])),
                    Width = img.Width,
                    Height = img.Height,
                });
            }
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
        private const int CLIP_HEIGHT = 480;
        private const int CLIP_PADDING = 200;
        #endregion
    }
}

