using System;

namespace BackgroundTest.TestConstructor
{
    class SlowCtorInterceptor : ISlowCtor
    {
        public SlowCtorInterceptor(Func<ISlowCtor> interceptionCallback)
        {
            Lazy = new Lazy<ISlowCtor>(interceptionCallback);
        }

        private Lazy<ISlowCtor> Lazy;

        public void InvokeObject()
        {
            Lazy.Value.InvokeObject();
        }
    }
}
