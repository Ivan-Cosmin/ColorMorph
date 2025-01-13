using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using SkiaSharp; // Pentru procesarea imaginilor
using System.IO;

namespace ColorMorph.Services
{
    public class ImageProcessingService
    {
        public static byte[] ApplyGrayscale(byte[] inputImageBytes)
        {
            try
            {
                using var mat = LoadMatFromBytes(inputImageBytes, ImreadModes.Color);
                using var grayMat = new Mat();
                CvInvoke.CvtColor(mat, grayMat, ColorConversion.Bgr2Gray);

                return ConvertMatToSkiaPng(grayMat);
            }
            catch (Exception ex)
            {
                // Handle the exception (log it, rethrow it, etc.)
                throw new ApplicationException("Error applying grayscale", ex);
            }
        }

        public static byte[] ApplyBlur(byte[] inputImageBytes)
        {
            try
            {
                using var mat = LoadMatFromBytes(inputImageBytes, ImreadModes.Color);
                using var blurredMat = new Mat();
                CvInvoke.GaussianBlur(mat, blurredMat, new System.Drawing.Size(15, 15), 0);

                return ConvertMatToSkiaPng(blurredMat);
            }
            catch (Exception ex)
            {
                // Handle the exception (log it, rethrow it, etc.)
                throw new ApplicationException("Error applying blur", ex);
            }
        }

        public static byte[] DetectEdges(byte[] inputImageBytes)
        {
            try
            {
                using var mat = LoadMatFromBytes(inputImageBytes, ImreadModes.Color);
                using var grayMat = new Mat();
                CvInvoke.CvtColor(mat, grayMat, ColorConversion.Bgr2Gray);

                using var edgeMat = new Mat();
                CvInvoke.Canny(grayMat, edgeMat, 100, 200);

                return ConvertMatToSkiaPng(edgeMat);
            }
            catch (Exception ex)
            {
                // Handle the exception (log it, rethrow it, etc.)
                throw new ApplicationException("Error detecting edges", ex);
            }
        }

        public static byte[] DetectFaces(byte[] inputImageBytes)
        {
            try
            {
                using var mat = LoadMatFromBytes(inputImageBytes, ImreadModes.Color);
                using var grayMat = new Mat();
                CvInvoke.CvtColor(mat, grayMat, ColorConversion.Bgr2Gray);

                var faceCascade = new CascadeClassifier("haarcascade_frontalface_default.xml");
                var faces = faceCascade.DetectMultiScale(grayMat, 1.1, 10, new System.Drawing.Size(20, 20));

                foreach (var face in faces)
                {
                    CvInvoke.Rectangle(mat, face, new MCvScalar(0, 255, 0), 2);
                }

                return ConvertMatToSkiaPng(mat);
            }
            catch (Exception ex)
            {
                // Handle the exception (log it, rethrow it, etc.)
                throw new ApplicationException("Error detecting faces", ex);
            }
        }

        public static byte[] Invert(byte[] inputImageBytes)
        {
            try
            {
                using var mat = LoadMatFromBytes(inputImageBytes, ImreadModes.Color);
                using var invertedMat = new Mat();
                CvInvoke.BitwiseNot(mat, invertedMat);

                return ConvertMatToSkiaPng(invertedMat);
            }
            catch (Exception ex)
            {
                // Handle the exception (log it, rethrow it, etc.)
                throw new ApplicationException("Error inverting image colors", ex);
            }
        }

        // Helper pentru încărcarea imaginii din byte[] în Mat
        private static Mat LoadMatFromBytes(byte[] imageBytes, ImreadModes mode)
        {
            try
            {
                var mat = new Mat();
                CvInvoke.Imdecode(imageBytes, mode, mat);
                return mat;
            }
            catch (Exception ex)
            {
                // Handle the exception (log it, rethrow it, etc.)
                throw new ApplicationException("Error loading image from bytes", ex);
            }
        }

        // Conversia Mat -> byte[] (PNG) folosind SkiaSharp
        private static byte[] ConvertMatToSkiaPng(Mat mat)
        {
            try
            {
                using var image = mat.ToImage<Bgr, byte>();
                var skBitmap = SKBitmap.Decode(image.ToJpegData());
                using var skImage = SKImage.FromBitmap(skBitmap);
                using var data = skImage.Encode(SKEncodedImageFormat.Png, 100);

                return data.ToArray();
            }
            catch (Exception ex)
            {
                // Handle the exception (log it, rethrow it, etc.)
                throw new ApplicationException("Error converting Mat to PNG", ex);
            }
        }
    }
}
