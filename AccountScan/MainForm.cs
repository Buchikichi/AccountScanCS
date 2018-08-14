using AccountScan.Data;
using AccountScan.Util;
using Emgu.CV;
using Emgu.CV.Structure;
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

        private Rectangle DetectTarget(PictureInfo info)
        {
            var region = new Rectangle();

            using (var img = info.Bitmap)
            using (var mat = new Image<Bgr, byte>(img).Mat)
            {
                var width = img.Width / 4;
                var maxY = img.Height - DETECT_HEIGHT;
                var y = 0;

                while (y < maxY)
                {
                    var rect = new Rectangle(DETECT_LEFT_MARGIN, y, width, DETECT_HEIGHT);
                    var hit = false;
                    var text = string.Empty;

                    region.X = 0;
                    SetStatusLabel($"y={y}");
                    using (var clip = new Mat(mat, rect))
                    {
                        var charList = OcrUtils.Recognize(clip, "jpn");

                        foreach (var ch in charList)
                        {
                            if (string.IsNullOrWhiteSpace(ch.Text) || !TARGET_STRING.Contains(ch.Text))
                            {
                                continue;
                            }
                            hit = true;
                            text += ch.Text;
                            CvInvoke.Rectangle(clip, ch.Region, new Bgr(Color.Green).MCvScalar);
                            if (region.X == 0)
                            {
                                region = ch.Region;
                            }
                            else
                            {
                                region = Rectangle.Union(region, ch.Region);
                            }
                        }
                        SetStatusLabel($"y={y}:[{text}]");
                        hit = text.Contains("\u5ea7\u767b");
                        if (hit)
                        {
                            CvInvoke.Imshow("source", clip);
                        }
                    }
                    if (hit)
                    {
                        region.Offset(DETECT_LEFT_MARGIN, y);
                        break;
                    }
                    y += DETECT_STEP;
                }
            }
            return region;
        }

        private void Detect()
        {
            var info = (PictureInfo)PictureListBox.SelectedItem;

            if (info == null)
            {
                return;
            }
            info.Region = DetectTarget(info);
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
        private const int DETECT_HEIGHT = 250;
        private const int DETECT_STEP = DETECT_HEIGHT / 2;
        private const int DETECT_LEFT_MARGIN = 250;
        private const int CLIP_HEIGHT = 450;
        private const string TARGET_STRING = "\u632f\u8fbc\u53e3\u5ea7\u767b\u9332";
        #endregion
    }
}
