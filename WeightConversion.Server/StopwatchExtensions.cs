using System;
using System.Diagnostics;

namespace WeightConversion.Server
{
    public static class StopwatchExtensions
    {
        public static long MeasureTimeElapsed(this Stopwatch sw, Action action)
        {
            sw.Reset();
            sw.Start();

            // execute the action passed in
            action();

            sw.Stop();

            Console.WriteLine("Elapsed: {0} ms.", sw.Elapsed.Milliseconds);
            return sw.ElapsedMilliseconds;
        }   


        //public static long MeasureTimeElapsed(this Stopwatch sw, Action action, int iterations)
        //{
        //    sw.Reset();
        //    sw.Start();
        //    for (int i = 0; i < iterations; i++)
        //    {
        //        action();
        //    }
        //    sw.Stop();

        //    return sw.ElapsedMilliseconds;
        //}
    }
}