using Emgu.CV;
using Emgu.CV.CvEnum;
using System.IO;

namespace ColorMorph.Services
{
    public class ImageProcessingService
    {
        //public static byte[] ApplyGrayscale(byte[] inputImageBytes)
        //{
        //    using var ms = new MemoryStream(inputImageBytes);
        //    using var mat = Mat.FromStream(ms, ImreadModes.Color);
        //    var grayMat = new Mat();
        //    CvInvoke.CvtColor(mat, grayMat, ColorConversion.Bgr2Gray);

        //    using var resultStream = new MemoryStream();
        //    grayMat.Bitmap.Save(resultStream, System.Drawing.Imaging.ImageFormat.Png);
        //    return resultStream.ToArray();
        //}

        //public static byte[] ApplyBlur(byte[] inputImageBytes)
        //{
        //    using var ms = new MemoryStream(inputImageBytes);
        //    using var mat = Mat.FromStream(ms, ImreadModes.Color);
        //    var blurredMat = new Mat();
        //    CvInvoke.GaussianBlur(mat, blurredMat, new System.Drawing.Size(15, 15), 0);

        //    using var resultStream = new MemoryStream();
        //    blurredMat.Bitmap.Save(resultStream, System.Drawing.Imaging.ImageFormat.Png);
        //    return resultStream.ToArray();
        //}
    }
}
