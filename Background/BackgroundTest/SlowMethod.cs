using System;
using System.Threading;

namespace BackgroundTest
{
    /// <summary>
    /// represent an instance with a slow operation
    /// </summary>
    public interface ISlowMethod
    {
        /// <summary>
        /// execute a slow operation
        /// </summary>
        void Perform();

        /// <summary>
        /// use for invoking a method on an object that has a slow operation
        /// </summary>
        void InvokeObject();
    }

    /// <summary>
    /// represent an instance with a slow operation
    /// </summary>
    public class SlowMethod : ISlowMethod
    {
        TimeSpan duration;

        public SlowMethod(TimeSpan duration)
        {
            this.duration = duration;
        }

        /// <summary>
        /// execute a slow operation
        /// </summary>
        public void Perform()
        {
            Thread.Sleep(duration);
        }

        /// <summary>
        /// use for invoking a method on an object that has a slow operation
        /// </summary>
        public void InvokeObject()
        {
            // assure this method wont be empty and ignored on running
            GC.KeepAlive(this);
        }
    }  
}
