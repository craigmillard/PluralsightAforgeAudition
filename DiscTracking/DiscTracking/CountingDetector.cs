using AForge.Vision.Motion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class CountingDetector : IMotionProcessing
    {
        void IMotionProcessing.ProcessFrame(AForge.Imaging.UnmanagedImage videoFrame, AForge.Imaging.UnmanagedImage motionFrame)
        {
            throw new NotImplementedException();

        }

        void IMotionProcessing.Reset()
        {
            throw new NotImplementedException();
        }
    }
}
