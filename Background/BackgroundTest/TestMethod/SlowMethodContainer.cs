using System;

namespace BackgroundTest.TestMethod
{
    /// <summary>
    /// A class containing an instance of type ISlowMethod that has a slow method
    /// </summary>
    public class SlowMethodContainer
    {
        ISlowMethod operation;

        public SlowMethodContainer(TimeSpan timeout)
        {
            operation = new SlowMethod(timeout);
        }

        public void InvokeSlowMethod()
        {
            operation.Perform();
        }

        public void InvokeContainee()
        {
            operation.InvokeObject();
        }
    }
}
