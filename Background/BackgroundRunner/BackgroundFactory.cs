using System;
using System.Linq.Expressions;
using Performance.Setters;
using System.Threading;

namespace Performance
{
    /// <summary>
    /// A Factory for starting a method or contrcurtor execution in background
    /// </summary>
    /// <typeparam name="T">The interface implemented by the instance</typeparam>
    public class Background<T> where T : class
    {
        #region Starting a method on backgound
        /// <summary>
        /// Start running long operation on background task while blocking all access to its target.
        /// After completion and when the first access to the objest is made, switch back the instance to the original one
        /// </summary>
        /// <param name="setter">Linq expression of the form  () => prop or () => field</param>
        /// <param name="longOperation">A long operation</param>
        /// <param name="interceptor">A Func that will be used to register a callback when the invoked target is called</param>
        /// <returns>A wrapper around the target object that will intercept his members invocation</returns>
        public static T StartMethod(Action longOperation, Expression<Func<T>> setter, Func<T, Action, T> methodInterceptor)
        {
            return StartMethod(longOperation, new LinqSetter<T>(setter), methodInterceptor);
        }

        /// <summary>
        /// Start running long operation on background task while blocking all access to its target.
        /// After completion and when the first access to the objest is made, switch back the instance to the original one
        /// </summary>
        /// <param name="setter">An action for setting back the instance back to the original holder</param>
        /// <param name="longOperation">A long operation</param>
        /// <param name="interceptor">A Func that will be used to register a callback when the invoked target is called</param>
        /// <returns>A wrapper around the target object that will intercept his members invocation</returns>
        public static T StartMethod(Action longOperation, Action setter, Func<T, Action, T> methodInterceptor)
        {
            return StartMethod(longOperation, new ActionSetter<T>(setter), methodInterceptor);
        }

        /// <summary>
        /// Start running long operation on background task while blocking all access to its target.
        /// After completion and when the first access to the objest is made, switch back the instance to the original one
        /// </summary>
        /// <param name="setter">A generic action for setting back the instance back to the original holder</param>
        /// <param name="longOperation">A long operation</param>
        /// <param name="interceptor">A Func that will be used to register a callback when the invoked target is called</param>
        /// <returns>A wrapper around the target object that will intercept his members invocation</returns>
        public static T StartMethod(Action longOperation, Action<T> setter, Func<T, Action, T> methodInterceptor)
        {
            return StartMethod(longOperation, new ActionSetter<T>(setter), methodInterceptor);
        }

        /// <summary>
        /// Start running long operation on background task while blocking all access to its target.
        /// After completion and when the first access to the objest is made, switch back the instance to the original one
        /// </summary>
        /// <param name="setter">A user defined way for setting back the instance back to the original holder</param>
        /// <param name="longOperation">A long operation</param>
        /// <param name="interceptor">A Func that will be used to register a callback when the invoked target is called</param>
        /// <returns>A wrapper around the target object that will intercept his members invocation</returns>
        public static T StartMethod(Action longOperation, ISetter<T> setter, Func<T, Action, T> methodInterceptor)
        {
            return StartMethodInternally(new BackgroundRunner<T>(longOperation, setter), (T)longOperation.Target, methodInterceptor);
        }

        #endregion

        #region Starting a Constructor on backgound
        /// <summary>
        /// Start running Constructor on background task while blocking all access to its members.
        /// After completion and when the first access to the objest is made, switch back the instance to the original one
        /// </summary>
        /// <param name="setter">A generic action for setting back the instance back to the original holder</param>
        /// <param name="longOperation">A long operation</param>
        /// <param name="interceptor">A Func that will be used to register a callback when the invoked target is called</param>
        /// <returns>A wrapper around the target object that will intercept his members invocation</returns>
        public static T StartCtor(Func<T> longCtor, Action<T> setter, Func<Func<T>, T> ctorInterceptor)
        {
            return StartCtor(longCtor, new ActionSetter<T>(setter), ctorInterceptor);
        }

        /// <summary>
        /// Start running Constructor on background task while blocking all access to its members.
        /// After completion and when the first access to the objest is made, switch back the instance to the original one
        /// </summary>
        /// <param name="setter">Linq expression of the form  () => prop or () => field</param>
        /// <param name="longOperation">A long operation</param>
        /// <param name="interceptor">A Func that will be used to register a callback when the invoked target is called</param>
        /// <returns>A wrapper around the target object that will intercept his members invocation</returns>
        public static T StartCtor(Func<T> longCtor, Expression<Func<T>> setter, Func<Func<T>, T> ctorInterceptor)
        {
            return StartCtor(longCtor, new LinqSetter<T>(setter), ctorInterceptor);
        }

        /// <summary>
        /// Start running Constructor on background task while blocking all access to its members.
        /// After completion and when the first access to the objest is made, switch back the instance to the original one
        /// </summary>
        /// <param name="setter">A user defined way for setting back the instance back to the original holder</param>
        /// <param name="longOperation">A long operation</param>
        /// <param name="interceptor">A Func that will be used to register a callback when the invoked target is called</param>
        /// <returns>A wrapper around the target object that will intercept his members invocation</returns>
        public static T StartCtor(Func<T> longCtor, ISetter<T> setter, Func<Func<T>, T> ctorInterceptor)
        {
            return StartCtorInternally(new BackgroundRunner<T>(longCtor, setter), ctorInterceptor);
        }
        #endregion
        
        private static T StartCtorInternally(BackgroundRunner<T> runner, Func<Func<T>, T> ctorInterceptor)
        {
            return StartInternally(ctorInterceptor(runner.IntercepteCtorCallback), runner);
        }

        private static T StartMethodInternally(BackgroundRunner<T> runner, T instance, Func<T, Action, T> methodInterceptor)
        {
            return StartInternally(methodInterceptor(instance, runner.IntercepteMethodCallback), runner);
        }

        private static T StartInternally(T intercepted, BackgroundRunner<T> runner)
        {
            // prevent instruction reordering that might cause background operation
            // to start and complete before we registered the callback
            Thread.MemoryBarrier();

            runner.RunOpOnBackground();
            return intercepted;
        }
    }
}
