using mshtml;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace ConvertHTMLToBootstrap.Models
{
    public class capture
    {
        public string imgCaptureImageUrl { get; set; }
        public void CaptureScreenShot(string url, int width, int height)
        {

            Thread thread = new Thread(delegate ()
            {
                WebBrowser wb = new WebBrowser();
                wb.ScrollBarsEnabled = false;
                wb.ScriptErrorsSuppressed = true;
                wb.Navigate(url);
                wb.Width = width;
                wb.Height = height;
                wb.Document.Body.SetAttribute("contentEditable", "true");

                wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DocumentCompleted);
                while (wb.ReadyState != WebBrowserReadyState.Complete)
                {
                    System.Windows.Forms.Application.DoEvents();
                }

                wb.Dispose();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
        // This code snippet is provided by GetCodeSnippet.com
        private void DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            

            WebBrowser wb = sender as WebBrowser;

            using (Bitmap bitmap = new Bitmap(wb.Width, wb.Height))
            {
                wb.DrawToBitmap(bitmap, new Rectangle(0, 0, wb.Width, wb.Height));
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    imgCaptureImageUrl = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }
        }

    }
    public class CaptureWebPage
    {
        private const string EXTRACTIMAGE_EXE = "IECapt.exe";
        private const int TIMEOUT = 60000;
        private const string TMP_NAME = "InBetween.png";

        public CaptureWebPage()
        {
        }

        private void Shot(string url, string rootDir)
        {
            Process p = new Process();
            p.StartInfo.FileName = rootDir + "\\" + EXTRACTIMAGE_EXE;
            p.StartInfo.Arguments = String.Format("\"{0}\" \"{1}\"", url, rootDir + "\\" + TMP_NAME);
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = false;
            p.Start();
            p.WaitForExit();
            p.Dispose();
        }

        private System.Drawing.Image Scale(System.Drawing.Image imgPhoto, int Width, int Height)
        {
            int srcWidth = imgPhoto.Width;
            int srcHeight = imgPhoto.Height;
            int srcX = 0; int srcY = 0;
            int destX = 0; int destY = 0;

            float percent = 0; float percentWidth = 0; float percentHeight = 0;

            percentWidth = ((float)Width / (float)srcWidth);
            percentHeight = ((float)Height / (float)srcHeight);

            if (percentHeight < percentWidth)
            {
                percent = percentWidth;
                destY = 0;
            }
            else
            {
                percent = percentHeight;
                destX = 0;
            }

            int destWidth = (int)(srcWidth * percent);
            int destHeight = (int)(srcHeight * percent);

            System.Drawing.Bitmap bmPhoto = new System.Drawing.Bitmap(Width,
                    Height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                    imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(srcX, srcY, srcWidth, srcHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public string GetImage(string url, string name, string rootDir, int width, int height)
        {
            string fileName = rootDir + "\\" + TMP_NAME;
            Shot(url, rootDir);
            System.Drawing.Image thumbImage = System.Drawing.Image.FromFile(fileName);
            Scale(thumbImage, width, height);
            System.Drawing.Image scaledImg = Scale(thumbImage, width, height);
            fileName = rootDir + "\\" + name + ".png";
            if (File.Exists(fileName))
                File.Delete(fileName);
            scaledImg.Save(fileName, ImageFormat.Png);
            return name + ".png";
        }
    }
}