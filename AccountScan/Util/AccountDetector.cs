using AccountScan.Data;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;

namespace AccountScan.Util
{
    class AccountDetector
    {
        #region Method
        private Rectangle Detect(Mat clip)
        {
            var region = default(Rectangle);
            var text = string.Empty;
            var charList = OcrUtils.Recognize(clip, "jpn");

            foreach (var ch in charList)
            {
                if (string.IsNullOrWhiteSpace(ch.Text) || !TARGET_STRING.Contains(ch.Text))
                {
                    continue;
                }
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
            if (!text.Contains("\u5ea7\u767b"))
            {
                region = default(Rectangle);
            }
            return region;
        }

        private Rectangle Detect(PictureInfo info, int y)
        {
            var detectWidth = (int)(info.Width * DETECT_HORIZONTAL_RATIO);
            var rect = new Rectangle(DETECT_LEFT_MARGIN, y, detectWidth, DETECT_HEIGHT);

            using (var img = info.Bitmap)
            using (var mat = new Image<Bgr, byte>(img).Mat)
            using (var clip = new Mat(mat, rect))
            {
                var region = Detect(clip);

                if (!region.IsEmpty)
                {
                    region.Offset(DETECT_LEFT_MARGIN, y);
                }
                return region;
            }
        }

        public Rectangle DetectTarget(PictureInfo info)
        {
            var region = new Rectangle();
            var taskList = new List<Task>();
            var detectArea = CalcDetectArea(info);
            var maxY = detectArea.Bottom - DETECT_STEP;

            for (var y = detectArea.Top; y < maxY; y += DETECT_STEP)
            {
                var localY = y;
                var task = Task.Run(() =>
                {
                    try
                    {
                        var rect = Detect(info, localY);

                        if (rect.IsEmpty || !region.IsEmpty && region.Bottom < rect.Bottom)
                        {
                            return;
                        }
                        region = rect;
                        //Debug.Print($"region[{rect}]{rect.Bottom}");
                    }
                    catch (Exception ex)
                    {
                        Debug.Print($"error[{ex.Message}]");
                    }
                });
                taskList.Add(task);
            }
            Task.WhenAll(taskList).Wait(10000);
            return region;
        }

        public Rectangle CalcDetectArea(PictureInfo info)
        {
            var width = (int)(info.Width * DETECT_HORIZONTAL_RATIO);
            var height = (int)(info.Height * DETECT_VERTICAL_RATIO);

            if (info.IsA4)
            {

                return new Rectangle(DETECT_LEFT_MARGIN, DETECT_TOP_MARGIN, width, height);
            }
            return new Rectangle(DETECT_LEFT_MARGIN, 0, width, height * 2);
        }
        #endregion

        #region Member
        private const int DETECT_HEIGHT = 250;
        private const int DETECT_STEP = DETECT_HEIGHT / 2;
        private const string TARGET_STRING = "\u632f\u8fbc\u53e3\u5ea7\u767b\u9332";

        private const int DETECT_TOP_MARGIN = 1500;
        private const int DETECT_LEFT_MARGIN = 250;
        private const double DETECT_HORIZONTAL_RATIO = .2;
        private const double DETECT_VERTICAL_RATIO = .18;
        #endregion
    }
}
