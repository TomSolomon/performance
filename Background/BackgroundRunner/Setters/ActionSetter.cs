using System;

namespace Performance.Setters
{
    /// <summary>
    /// Providing an way to set the original\newly created instance back to its container using action
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ActionSetter<T> : ISetter<T>
    {
        private Action<T> setBackAction;

        public ActionSetter(Action<T> setBackAction)
        {
            this.setBackAction = setBackAction;
        }
        
        public ActionSetter(Action setBackAction) 
            : this( (x) => setBackAction() )
        {}

        public void SetBack(T instance)
        {
            setBackAction(instance);
        }
    }
}
