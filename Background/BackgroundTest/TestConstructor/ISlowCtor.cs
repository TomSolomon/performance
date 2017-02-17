using System;

namespace BackgroundTest.TestConstructor
{
    public interface ISlowCtor
    {
        /// <summary>
        /// use for invoking a method on the object in order to test it
        /// </summary>
        void InvokeObject();
    }

    class SlowCtor : ISlowCtor
    {
        public SlowCtor(TimeSpan operationDuration)
        {
            new SlowMethod(operationDuration).Perform();
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