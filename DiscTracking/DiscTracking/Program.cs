using AForge;
using AForge.Imaging.Filters;
using AForge.Video.FFMPEG;
using AForge.Vision.Motion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Program
    {
        public static AForge.Vision.Motion.MotionDetector motionDetector = null;
        public static AForge.Vision.Motion.BlobCountingObjectsProcessing processor = null;
        public static List<Rectangle> motionDetectedZones = new List<Rectangle>();

        static void Main(string[] args)
        {
            VideoFileReader reader = new VideoFileReader();
            reader.Open(@"C:\Development\PluralsightAforgeAudition\DiscThrow.mp4");
            MotionDetector detector = GetDefaultMotionDetector();

            // Zone to look for motion in
            Rectangle[] zones = new Rectangle[1];
            zones[0] = new Rectangle(490, 250, 395, 470);
            detector.MotionZones = zones;

            // Looping through each frame in the video (At 30 fps it's about ~4 seconds)
            for (int i = 0; i < 126; i++)
            {
                Bitmap processedFrame = reader.ReadVideoFrame();
                var originalFrame = new Bitmap(processedFrame);
                float current = 0;

                // Blur, then detect motion (helps elminate background "noise")
                GaussianBlur blur = new GaussianBlur(3, 3);
                blur.ApplyInPlace(processedFrame);
                current = detector.ProcessFrame(processedFrame);

                // Draw rectangles around detected motion
                Graphics g = Graphics.FromImage(originalFrame);
                if (processor.ObjectsCount > 0)
                {
                    motionDetectedZones.AddRange(processor.ObjectRectangles);
                    DrawMotion(g, processor.ObjectRectangles);
                }

                // Save the processed frame as an image
                originalFrame.Save(@"C:\Development\PluralsightAforgeAudition\video\" + i.ToString("D5") + ".png");
                processedFrame.Dispose();

            }

            reader.Close();

        }

        public static AForge.Vision.Motion.MotionDetector GetDefaultMotionDetector()
        {
            var detector = new AForge.Vision.Motion.TwoFramesDifferenceDetector()
            {
                DifferenceThreshold = 4,
                SuppressNoise = true
            };


            processor = new AForge.Vision.Motion.BlobCountingObjectsProcessing()
            {
                HighlightColor = System.Drawing.Color.LightSeaGreen,
                HighlightMotionRegions = true,
                MinObjectsHeight = 7,
                MinObjectsWidth = 7,
            };

            motionDetector = new AForge.Vision.Motion.MotionDetector(detector, processor);
            return (motionDetector);
        }

        public static void DrawMotion(Graphics g, IEnumerable<Rectangle> motionZones)
        {
            Pen greenPen = new Pen(Color.LightGreen, 3);

            foreach (var rect in motionZones)
            {
                g.DrawRectangle(greenPen, rect);
            }
        }


    }
}
