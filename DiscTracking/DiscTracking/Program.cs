﻿using AForge;
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
            // check some of its attributes
            Console.WriteLine("width:  " + reader.Width);
            Console.WriteLine("height: " + reader.Height);
            Console.WriteLine("fps:    " + reader.FrameRate);
            Console.WriteLine("codec:  " + reader.CodecName);
            // read 100 video frames out of it




            MotionDetector detector = GetDefaultMotionDetector();
            
            int j = 0;
            Rectangle[] zones = new Rectangle[2];
            zones[0] = new Rectangle(0, 0, 1279, 400);
            zones[1] = new Rectangle(520, 0, 210, 499);

            detector.MotionZones = zones;


            for (int i = 0; i < 120; i++)
            {
                Bitmap videoFrame = reader.ReadVideoFrame();
                float current = 0;

                    GaussianBlur blur = new GaussianBlur(2, 2);
                    //blur.ApplyInPlace(videoFrame);

                    current = detector.ProcessFrame(videoFrame);
                    
                   
        
                    //ColorFiltering filter = new ColorFiltering();
                    //// set color ranges to keep
                    //filter.Red = new IntRange(160, 255);
                    //filter.Green = new IntRange(140, 220);
                    //filter.Blue = new IntRange(140, 220);
                    //// apply the filter
                    //filter.ApplyInPlace(videoFrame);



                    Graphics g = Graphics.FromImage(videoFrame);
                    videoFrame.Save(@"C:\Development\PluralsightAforgeAudition\Output\" + i.ToString("D5") + ".png");


                    // process the frame somehow
                    // ...


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

            //detector = new AForge.Vision.Motion.CustomFrameDifferenceDetector()
            //{
            //    DifferenceThreshold = 7,
            //    KeepObjectsEdges = true,
            //    SuppressNoise = true
            //};

            //detector = new AForge.Vision.Motion.SimpleBackgroundModelingDetector()
            //{
            //    DifferenceThreshold = 8,
            //    FramesPerBackgroundUpdate = 20,
            //    KeepObjectsEdges = true,
            //    MillisecondsPerBackgroundUpdate = 1000,
            //    SuppressNoise = true
            //};

            //processor = new AForge.Vision.Motion.GridMotionAreaProcessing()
            //{
            //    HighlightColor = System.Drawing.Color.Red,
            //    HighlightMotionGrid = true,
            //    GridWidth = 1000,
            //    GridHeight = 1000,
            //    MotionAmountToHighlight = 10
            //};

            processor = new AForge.Vision.Motion.BlobCountingObjectsProcessing()
            {
                HighlightColor = System.Drawing.Color.LightYellow,
                HighlightMotionRegions = true,
                MinObjectsHeight = 5,
                MinObjectsWidth = 5,
            };



            motionDetector = new AForge.Vision.Motion.MotionDetector(detector, processor);

            return (motionDetector);
        }
    }
}
