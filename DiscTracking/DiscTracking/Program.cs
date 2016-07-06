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
        public static AForge.Vision.Motion.IMotionDetector detector = null;
        public static AForge.Vision.Motion.IMotionProcessing processor = null;
        public static AForge.Vision.Motion.MotionDetector motionDetector = null;

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


            for (int i = 0; i < 120; i++)
            {
                Bitmap videoFrame = reader.ReadVideoFrame();
                float current = 0;

                GaussianBlur blur = new GaussianBlur(3, 3);
                blur.ApplyInPlace(videoFrame);

                current = detector.ProcessFrame(videoFrame);
                Graphics g = Graphics.FromImage(videoFrame);
                Pen blackPen = new Pen(Color.Black, 3);
                g.DrawRectangle(blackPen, zones[0]);
                videoFrame.Save(@"C:\Development\PluralsightAforgeAudition\Output\" + i.ToString("D5") + ".png");
                videoFrame.Dispose();

            }
            reader.Close();

        }

        public static AForge.Vision.Motion.MotionDetector GetDefaultMotionDetector()
        {
            detector = new AForge.Vision.Motion.TwoFramesDifferenceDetector()
            {
                DifferenceThreshold = 4,
                SuppressNoise = true
            };


            processor = new AForge.Vision.Motion.BlobCountingObjectsProcessing()
            {
                HighlightColor = System.Drawing.Color.LightYellow,
                HighlightMotionRegions = true,
                MinObjectsHeight = 7,
                MinObjectsWidth = 7,
            };

            motionDetector = new AForge.Vision.Motion.MotionDetector(detector, processor);
            return (motionDetector);
        }
    }
}
