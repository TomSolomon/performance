using System;

namespace BackgroundTest.TestMethod
{    
    /// <summary>
    /// an interceptor of ISlowMethod. in this example the instance we intercepting is given using interceptCallback
    /// </summary>
    public class SlowMethodInterceptor : ISlowMethod
    {
        Action interceptCallback;
        ISlowMethod instance; 

        public SlowMethodInterceptor(ISlowMethod instance, Action interceptCallback)
        {
            this.interceptCallback = interceptCallback;
            this.instance = instance;
        }

        public void Perform()
        {
            interceptCallback();
            instance.Perform();
        }

        public void InvokeObject()
        {
            interceptCallback();
            instance.InvokeObject();
        }
    }
}
