using System;
using System.Diagnostics;

namespace BackgroundTest
{
    public class TimeoutUtil
    {
        public static readonly TimeSpan defaultTimeoutGranularity = TimeSpan.FromMilliseconds(150);

        /// <summary>
        /// Test the if the given action execute immidiately (wit default time granularity)
        /// </summary>      
        /// <param name="action">the method to test</param>
        /// <exception cref="TimeoutException">Throw on timeout valioation</exception>
        public static void TestImmidiate(Action action)
        {
            Test(action, TimeSpan.Zero);
        }

        /// <summary>
        /// Test the if the given action execute time is expectedTimeSpan (with default time granularity)
        /// </summary>
        /// <param name="action">the method to test</param>
        /// <param name="expectedTimeSpan">The expected time to execute the ation</param>
        /// <exception cref="TimeoutException">Throw on timeout valioation</exception>
        public static void Test(Action action, TimeSpan expectedTimeSpan)
        {
            Test(action, expectedTimeSpan, defaultTimeoutGranularity);
        }

        /// <summary>
        /// Test the if the given action execute time is expectedTimeSpan (with timeSpanGranularity time granularity)
        /// </summary>
        /// <param name="action">the method to test</param>
        /// <param name="expectedTimeSpan">The expected time to execute the ation</param>
        /// <param name="timeSpanGranularity">the time ganularity</param>
        /// <exception cref="TimeoutException">Throw on timeout valioation</exception>
        public static void Test(Action action, TimeSpan expectedTimeSpan, TimeSpan timeSpanGranularity)
        {
            var sw = Stopwatch.StartNew();
            action();
            sw.Stop();
            if ((sw.Elapsed - expectedTimeSpan).Duration() > timeSpanGranularity)
            {
                throw new TimeoutException(string.Format(
                    "operation took {0}[MS] while expecting it to take {1}[MS]", sw.Elapsed, expectedTimeSpan));
            }      
        }       
    }

    static class TimeSpanUtils
    {
        public static TimeSpan Multiply(this TimeSpan timeSpane, double mul)
        {
            return TimeSpan.FromTicks((long)(timeSpane.Ticks * mul));
        }

        public static TimeSpan Multiply(this TimeSpan timeSpane, int mul)
        {
            return TimeSpan.FromTicks(timeSpane.Ticks * mul);
        }

        public static TimeSpan Half(this TimeSpan timeSpane)
        {
            return timeSpane.Multiply(0.5);
        }      
    }
}
