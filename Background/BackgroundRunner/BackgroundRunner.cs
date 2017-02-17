using Performance.Setters;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Performance
{
    /// <summary>
    /// Run an operation in background while delaying all attempts to access the target
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class BackgroundRunner<T> where T : class
    {
        // the object we invoking longOperation on
        T target;

        Func<T> longOperation;
        
        ISetter<T> setter;

        // use to synchronize access to the target
        ManualResetEventSlim resetEvent;

        /// <summary>
        /// To delay any access to the object while its cTor is invoked in the background, 
        /// any call to it is intercepted by this method and blocked until background operation completes. 
        /// At the end of the background operation, the object is returned
        /// </summary>
        /// <returns>Return a reference to object when its cTor completes</returns>
        public T IntercepteCtorCallback()
        {
            IntercepteMethodCallback();
            return target;
        }

        /// <summary>
        /// To delay any access to the object while one of his methods\properties is invoked in the background, 
        /// any call to it is intercepted by this method and blocked until background operation completes. 
        /// </summary>
        public void IntercepteMethodCallback()
        {
            if (target == null)
            {
                resetEvent.Wait();
                resetEvent.Dispose();
                resetEvent = null;
            }

            Debug.Assert(target != null, "target should have been set by long operation before signaling the event");

            // making sure we will set back the reference only once
            if (setter != null)
            {
                setter.SetBack(target);
                setter = null;
            }
        }

        public BackgroundRunner(Action longOperation, ISetter<T> setter)
            : this(() => { longOperation(); return (T)longOperation.Target; }, setter)
        {}

        public BackgroundRunner(Func<T> longOperation, ISetter<T> setter)
        {
            this.setter = setter;
            this.longOperation = longOperation;
            resetEvent = new ManualResetEventSlim();
        }

        public void RunOpOnBackground()
        {
            Task.Factory.StartNew(() =>
            {
                target = longOperation();
                resetEvent.Set();
            });
        }        
    }
}
