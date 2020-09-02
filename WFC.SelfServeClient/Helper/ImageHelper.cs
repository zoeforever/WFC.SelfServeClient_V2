using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PixelFormat = System.Windows.Media.PixelFormat;

namespace WFC.SelfServeClient.Helper
{
    public class ImageHelper
    {  /// <summary>
       /// 根据图片字节信息返回图片
       /// </summary>
       /// <param name="imageData">图片字节信息</param>
       /// <returns></returns>
        public static ImageSource BytesToImageSource(byte[] imageData)
        {
            if (imageData != null)
            {
                try
                {
                    BitmapImage showImage = new BitmapImage();
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        showImage.BeginInit();
                        showImage.CacheOption = BitmapCacheOption.OnLoad;
                        showImage.CreateOptions = BitmapCreateOptions.None;
                        showImage.StreamSource = ms;
                        showImage.EndInit();
                        showImage.Freeze();
                    }
                    return showImage;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("BytesToImageSource:" + ex.ToString());
                }
            }
            System.Diagnostics.Trace.WriteLine("BytesToImageSource return null");
            return null;
        }

        public static void SaveBitmapImageToFile(string path, BitmapImage image)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var fileStream = new System.IO.FileStream(path, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        public static void SaveImageDataToFile(string path, byte[] imgData)
        {
            using (var fileStream = new System.IO.FileStream(path, System.IO.FileMode.Create))
            {
                fileStream.Write(imgData, 0, imgData.Length);
                fileStream.Flush();
            }
        }

        public static byte[] GetBytesFromInteropBitmap(System.Windows.Interop.InteropBitmap ibmp)
        {
            BitmapSource bmpSource = ibmp as BitmapSource;
            using (MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmpSource));
                encoder.Save(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return ms.GetBuffer();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] BitmapImageToBytes(BitmapImage image)
        {
            byte[] imageData = null;
            try
            {
                Stream smarket = image.StreamSource;
                if (smarket != null && smarket.Length > 0)
                {
                    //很重要，因为position经常位于stream的末尾，导致下面读取到的长度为0。
                    smarket.Position = 0;

                    using (BinaryReader br = new BinaryReader(smarket))
                    {
                        imageData = br.ReadBytes((int)smarket.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("BitmapImageToBytes:" + ex.ToString());
            }
            System.Diagnostics.Trace.WriteLine("BitmapImageToBytes.imageData:" + imageData == null ? "null" : imageData.Length.ToString());
            return imageData;
        }

        /// <summary>
        /// 根据指向RGB格式数据的指针生成图片，切片函数使用
        /// </summary>
        /// <param name="ptrRGB"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static ImageSource GetImage(IntPtr ptrRGB, int width, int height, int newWidth, int newHeight)
        {
            int length = width * height * 4; //ptrRGB里面存的是argb格式的数据，所以要*4
            byte[] imageData = new byte[length];
            Marshal.Copy(ptrRGB, imageData, 0, length);
            return GetSliceImage(imageData, width, height, newWidth, newHeight);
        }
        /// <summary>
        /// 根据指向RGB格式数据的指针生成图片，切片函数使用
        /// </summary>
        /// <param name="ptrRGB"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static ImageSource GetImage(IntPtr ptrRGB, int width, int height)
        {
            return GetImage(ptrRGB, width, height, width, height);
        }

        public static BitmapSource GetSliceImage(byte[] imageDataRGB, int width, int height)
        {
            //var dpiX = 96d;
            //var dpiY = 96d;
            //var pixelFormat = PixelFormats.Rgb24; // grayscale bitmap
            //var bytesPerPixel = (pixelFormat.BitsPerPixel + 7) / 8; // == 1 in this example
            //int stride = bytesPerPixel * width; // == width in this example
            //byte[] rgbValues = new byte[stride * height];
            //int i = 0;
            //for (int h = 0; h < height; h++)
            //{
            //    for (int w = 0; w < width; w++)
            //    {
            //        byte r = imageDataRGB[i++];
            //        byte g = imageDataRGB[i++];
            //        byte b = imageDataRGB[i++];
            //        byte a = imageDataRGB[i++];

            //        rgbValues[h * stride + w * 3] = (byte)b;
            //        rgbValues[h * stride + w * 3 + 1] = (byte)g;
            //        rgbValues[h * stride + w * 3 + 2] = (byte)r;
            //    }
            //}
            //return BitmapSource.Create(width, height, dpiX, dpiY,
            //                                        pixelFormat, null, rgbValues, stride);
            return GetSliceImage(imageDataRGB, width, height, width, height);
        }

        public static BitmapSource GetSliceImage(byte[] imageDataRGB, int width, int height, int newWidth, int newHeight)
        {
            var dpiX = 96d;
            var dpiY = 96d;
            var pixelFormat = PixelFormats.Rgb24;
            var bytesPerPixel = (pixelFormat.BitsPerPixel + 7) / 8;
            int stride = bytesPerPixel * width;
            byte[] rgbValues = new byte[stride * height];
            int i = 0;
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    byte r = imageDataRGB[i++];
                    byte g = imageDataRGB[i++];
                    byte b = imageDataRGB[i++];
                    byte a = imageDataRGB[i++];

                    rgbValues[h * stride + w * 3] = (byte)r;
                    rgbValues[h * stride + w * 3 + 1] = (byte)g;
                    rgbValues[h * stride + w * 3 + 2] = (byte)b;
                }
            }
            //if (width == newWidth && height == newHeight)
            //{
            //    return BitmapSource.Create(width, height, dpiX, dpiY,
            //                                            pixelFormat, null, rgbValues, stride);
            //}
            //else
            //{
            IntPtr ptr;
            var bmp = ImageBytesToBitmap(rgbValues, width, height, out ptr);
            var bp = ResizeImage(bmp, newWidth, newHeight);
            Marshal.FreeHGlobal(ptr);
            BitmapSource returnSource;
            try
            {
                returnSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("GetSliceImage:" + ex.ToString());
                returnSource = null;
            }
            return returnSource;
            //}
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="mg"></param>
        /// <param name="width">缩放后的宽度</param>
        /// <param name="height">缩放后的高度</param>
        /// <returns></returns>
        private static Bitmap ResizeImage(Bitmap mg, int width, int height)
        {
            double ratio = 0d;
            double myThumbWidth = 0d;
            double myThumbHeight = 0d;
            int x = 0;
            int y = 0;

            Bitmap bp;

            if ((mg.Width / Convert.ToDouble(width)) > (mg.Height /
            Convert.ToDouble(height)))
                ratio = Convert.ToDouble(mg.Width) / Convert.ToDouble(width);
            else
                ratio = Convert.ToDouble(mg.Height) / Convert.ToDouble(height);
            myThumbHeight = Math.Ceiling(mg.Height / ratio);
            myThumbWidth = Math.Ceiling(mg.Width / ratio);

            //Size thumbSize = new Size((int)myThumbWidth, (int)myThumbHeight);
            var thumbSize = new System.Drawing.Size((int)width, (int)height);
            bp = new Bitmap(width, height);
            x = (width - thumbSize.Width) / 2;
            y = (height - thumbSize.Height);
            // Had to add System.Drawing class in front of Graphics ---
            System.Drawing.Graphics g = Graphics.FromImage(bp);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Rectangle rect = new Rectangle(x, y, thumbSize.Width, thumbSize.Height);
            g.DrawImage(mg, rect, 0, 0, mg.Width, mg.Height, GraphicsUnit.Pixel);
            g.Dispose();
            return bp;
        }

        /// <summary>
        /// 根据指向RGB格式数据的指针生成图片，切片函数使用
        /// </summary>
        /// <param name="ptrRGB"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static ImageSource GetImage(byte[] imageDataRGB, int width, int height)
        {
            try
            {
                var dpiX = 96d;
                var dpiY = 96d;
                var pixelFormat = PixelFormats.Rgb24;
                var bytesPerPixel = (pixelFormat.BitsPerPixel + 7) / 8;
                int stride = (width * bytesPerPixel + 3) / 4 * 4;
                byte[] rgbValues = new byte[stride * height];
                int i = 0;
                for (int h = 0; h < height; h++)
                {
                    for (int w = 0; w < width; w++)
                    {
                        byte r = imageDataRGB[i++];
                        byte g = imageDataRGB[i++];
                        byte b = imageDataRGB[i++];
                        rgbValues[h * stride + w * 3] = imageDataRGB[h * stride + w * 3 + 2];
                        rgbValues[h * stride + w * 3 + 1] = imageDataRGB[h * stride + w * 3 + 1];
                        rgbValues[h * stride + w * 3 + 2] = imageDataRGB[h * stride + w * 3];
                    }
                }
                return BitmapSource.Create(width, height, dpiX, dpiY,
                                                        pixelFormat, null, rgbValues, stride);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("GetImage:" + ex.ToString());
                return null;
            }
        }
        public static ImageSource GetImage(Bitmap bitmap)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public static ImageSource GetImage(byte[] imageDataRGB, int width, int height, PixelFormat pixelFormat)
        {
            try
            {
                var dpiX = 96d;
                var dpiY = 96d;
                var bytesPerPixel = (pixelFormat.BitsPerPixel + 7) / 8;
                int stride = (width * bytesPerPixel + 3) / 4 * 4; ;
                if (pixelFormat == PixelFormats.Gray8)
                {
                    return BitmapSource.Create(width, height, dpiX, dpiY,
                                                      pixelFormat, null, imageDataRGB, stride);
                }
                else
                {
                    byte[] rgbValues = new byte[stride * height];
                    int i = 0;
                    for (int h = 0; h < height; h++)
                    {
                        for (int w = 0; w < width; w++)
                        {
                            byte r = imageDataRGB[i++];
                            byte g = imageDataRGB[i++];
                            byte b = imageDataRGB[i++];
                            rgbValues[h * stride + w * 3] = imageDataRGB[h * stride + w * 3 + 2];
                            rgbValues[h * stride + w * 3 + 1] = imageDataRGB[h * stride + w * 3 + 1];
                            rgbValues[h * stride + w * 3 + 2] = imageDataRGB[h * stride + w * 3];
                        }
                    }
                    return BitmapSource.Create(width, height, dpiX, dpiY,
                                                            pixelFormat, null, rgbValues, stride);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("GetImage:" + ex.ToString());
                return null;
            }
        }

        public static byte[] GetAllBytesFromBitmap(Bitmap bitmap)
        {
            string tmpFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString() + ".jpg");
            bitmap.Save(tmpFile);
            var bytes = GetBytesByImagePath(tmpFile);
            try
            {
                File.Delete(tmpFile);
            }
            catch { }
            return bytes;
        }
        /// <summary>
        /// 将bitmap图像中的RGB像素提取出来，作为二进制数组返回，格式为RGB……
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] BitmapToImageBytes(Bitmap bitmap)
        {
            var data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                     System.Drawing.Imaging.ImageLockMode.ReadOnly,
                     System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            byte[] imgBytes = new byte[data.Stride * bitmap.Height];
            try
            {
                byte[] pixelData = new Byte[data.Stride];
                for (int scanline = 0; scanline < data.Height; scanline++)
                {
                    Marshal.Copy(data.Scan0 + (scanline * data.Stride), pixelData, 0, data.Stride);
                    for (int pixeloffset = 0; pixeloffset < data.Width; pixeloffset++)
                    {
                        imgBytes[(scanline * data.Stride) + pixeloffset * 3] = pixelData[pixeloffset * 3];
                        imgBytes[(scanline * data.Stride) + pixeloffset * 3 + 1] = pixelData[pixeloffset * 3 + 1];
                        imgBytes[(scanline * data.Stride) + pixeloffset * 3 + 2] = pixelData[pixeloffset * 3 + 2];
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("BitmapToImageBytes:" + ex.ToString());
            }
            finally
            {
                bitmap.UnlockBits(data);
            }
            return imgBytes;
        }

        /// <summary>
        /// 将二进制数组数据生成Bitmap对象
        /// </summary>
        /// <param name="imageData"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap ImageBytesToBitmap(byte[] imageData, int width, int height)
        {
            int stride = (width * 3 + 3) / 4 * 4;
            IntPtr ptr = Marshal.AllocHGlobal(imageData.Length);
            Marshal.Copy(imageData, 0, ptr, imageData.Length);
            Bitmap bitmap = new Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format24bppRgb, ptr);
            return bitmap;
        }

        /// <summary>
        /// 将二进制数组数据生成Bitmap对象
        /// </summary>
        /// <param name="imageData">图像数据</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="ptr">指向图像的IntPtr指针，应该在用完返回值之后释放，否则会内存泄漏</param>
        /// <returns></returns>
        public static Bitmap ImageBytesToBitmap(byte[] imageData, int width, int height, out IntPtr ptr)
        {
            int stride = (width * 3 + 3) / 4 * 4;
            ptr = Marshal.AllocHGlobal(imageData.Length);
            Marshal.Copy(imageData, 0, ptr, imageData.Length);
            Bitmap bitmap = new Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format24bppRgb, ptr);
            return bitmap;
        }

        /// <summary>
        /// 将实际位置中的照片转化为byte[]类型写入数据库中
        /// </summary>
        /// <param name="strFile">string图片地址</param>
        /// <returns>byte[]</returns>
        public static byte[] GetBytesByImagePath(string strFile)
        {
            byte[] photo_byte = null;
            try
            {
                using (FileStream fs = new FileStream(strFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        photo_byte = br.ReadBytes((int)fs.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("GetBytesByImagePath:" + ex.ToString());
            }
            return photo_byte;
        }

        /// <summary>
        /// 根据图片字节数组获取BitmapImage
        /// </summary>
        /// <param name="imgData"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static BitmapImage Create(byte[] imgData, int width = 150, int height = 150)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;//32系统下，某些图片不显示
            img.DecodePixelWidth = width;
            img.DecodePixelHeight = height;
            img.StreamSource = new MemoryStream(imgData);
            img.EndInit();
            return img;
        }

        public static BitmapImage Create(string url)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;//32系统下，某些图片不显示
            img.UriSource = new Uri(url);
            img.EndInit();
            return img;
        }


        /// <summary>
        /// 图片等比缩放
        /// </summary>
        /// <param name="fromFile">原图Stream对象</param>
        /// <param name="savePath">缩略图存放地址</param>
        /// <param name="targetWidth">指定的最大宽度</param>
        /// <param name="targetHeight">指定的最大高度</param>
        public static byte[] ZoomAuto(byte[] fromFile, System.Double targetWidth, System.Double targetHeight)
        {
            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            using (MemoryStream ori = new MemoryStream(fromFile))
            {
                try
                {
                    System.Drawing.Image initImage = System.Drawing.Image.FromStream(ori, true);
                    //原图宽高均小于模版，不作处理，直接保存
                    if (initImage.Width <= targetWidth && initImage.Height <= targetHeight)
                    {
                        using (var ms = new MemoryStream())
                        {
                            //保存
                            initImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            byte[] buffer = new byte[ms.Length];
                            //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                            ms.Seek(0, SeekOrigin.Begin);
                            ms.Read(buffer, 0, buffer.Length);
                            return buffer;
                        }
                    }
                    else
                    {
                        //缩略图宽、高计算
                        double newWidth = initImage.Width;
                        double newHeight = initImage.Height;

                        //宽大于高或宽等于高（横图或正方）
                        if (initImage.Width > initImage.Height || initImage.Width == initImage.Height)
                        {
                            //如果宽大于模版
                            if (initImage.Width > targetWidth)
                            {
                                //宽按模版，高按比例缩放
                                newWidth = targetWidth;
                                newHeight = initImage.Height * (targetWidth / initImage.Width);
                            }
                        }
                        //高大于宽（竖图）
                        else
                        {
                            //如果高大于模版
                            if (initImage.Height > targetHeight)
                            {
                                //高按模版，宽按比例缩放
                                newHeight = targetHeight;
                                newWidth = initImage.Width * (targetHeight / initImage.Height);
                            }
                        }

                        //生成新图
                        //新建一个bmp图片
                        System.Drawing.Image newImage = new System.Drawing.Bitmap((int)newWidth, (int)newHeight);
                        //新建一个画板
                        System.Drawing.Graphics newG = System.Drawing.Graphics.FromImage(newImage);

                        //设置质量
                        newG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        newG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        //置背景色
                        newG.Clear(System.Drawing.Color.White);
                        //画图
                        newG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, newImage.Width, newImage.Height), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);
                        byte[] buffer = null;
                        using (var ms = new MemoryStream())
                        {
                            //保存
                            newImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            buffer = new byte[ms.Length];
                            //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                            ms.Seek(0, SeekOrigin.Begin);
                            ms.Read(buffer, 0, buffer.Length);
                        }

                        //释放资源
                        newG.Dispose();
                        newImage.Dispose();
                        initImage.Dispose();
                        return buffer;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("ZoomAuto:" + ex.ToString());
                    return null;
                }
            }
        }


        public static BitmapImage CreateOriginal(byte[] imgData)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;//32系统下，某些图片不显示
            img.StreamSource = new MemoryStream(imgData);
            img.EndInit();
            return img;
        }
    }
}
