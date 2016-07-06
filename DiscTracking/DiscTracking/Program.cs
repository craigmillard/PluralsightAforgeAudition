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
            // create instance of video reader
            VideoFileReader reader = new VideoFileReader();
            // open video file
            reader.Open(@"C:\Development\PluralsightAforgeAudition\DiscThrow.mp4");

            MotionDetector detector = GetDefaultMotionDetector();

            Rectangle[] zones = new Rectangle[1];
            zones[0] = new Rectangle(490, 250, 395, 470);
            detector.MotionZones = zones;           

            for (int i = 0; i < 126; i++)
            {
                Bitmap videoFrame = reader.ReadVideoFrame();
                var processedFrame = new Bitmap(videoFrame);
                float current = 0;

                // Process Frame
                GaussianBlur blur = new GaussianBlur(3, 3);
                blur.ApplyInPlace(videoFrame);
                current = detector.ProcessFrame(videoFrame);

                Graphics g = Graphics.FromImage(processedFrame);
                if (processor.ObjectsCount > 0)
                {
                    motionDetectedZones.AddRange(processor.ObjectRectangles);
                    DrawMotion(g, processor.ObjectRectangles);
                }


                //Show last frame
                if (i == 125)
                {
                    DrawMotion(g, motionDetectedZones);
                }

                processedFrame.Save(@"C:\Development\PluralsightAforgeAudition\video\" + i.ToString("D5") + ".png");
                videoFrame.Dispose();

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
                //g.FillEllipse(greenPen.Brush, rect);
            }
        }


    }
}
