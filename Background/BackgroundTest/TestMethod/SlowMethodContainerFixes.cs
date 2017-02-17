using Performance;
using Performance.Setters;
using System;

namespace BackgroundTest.TestMethod
{
    /// <summary>
    /// several common mechanisms for the upoming fixes
    /// </summary>
    public class SlowMethodContainerFix
    {
        protected Func<ISlowMethod, Action, ISlowMethod> methodInterception =
          (instance, callback) => new SlowMethodInterceptor(instance, callback);

        protected ISlowMethod operation;

        protected SlowMethodContainerFix(TimeSpan timeout)
        {
            operation = new SlowMethod(timeout);
        }
    
        public void InvokeContainee()
        {
            operation.InvokeObject();
        }
    }


    /// <summary>
    /// Demonstrate using an action, a generic actionm, a field in linq expression to set back the instance
    /// </summary>
    public class SlowMethodContainerFix1 : SlowMethodContainerFix
    {       
        public SlowMethodContainerFix1(TimeSpan timeout) : base(timeout)
        { }    

        public void FixUsingAction()
        {
            // for this example we will imitate a creation of SlowOperation localy and passing a reference to the newly created one
            // and using it to set back to new value
            ISlowMethod localRef = operation;//new SlowOperation(timeout);
            operation = Background<ISlowMethod>.StartMethod((Action)(() => operation = localRef), localRef.Perform, methodInterception);
        }

        public void FixUsingFieldLinqExpression()
        {
            operation = Background<ISlowMethod>.StartMethod(() => operation, operation.Perform, methodInterception);
        }

        public void FixUsingGenericAction()
        {
            operation = Background<ISlowMethod>.StartMethod((newVal) => operation = newVal, operation.Perform, methodInterception);
        }
    }

    /// <summary>
    /// Demonstrate using a user defined setter to set back the instance
    /// </summary>
    public class SlowMethodContainerFix2 : SlowMethodContainerFix, ISetter<ISlowMethod>
    {       
        public SlowMethodContainerFix2(TimeSpan timeout) : base(timeout)
        {}

        public void FixUsingUserDefinedSetter()
        {
            operation = Background<ISlowMethod>.StartMethod(this, operation.Perform, methodInterception);
        }

        public void SetBack(ISlowMethod instance)
        {
            operation = instance;
        }      
    }

    /// <summary>
    /// Demonstrate using an operation in a linq expression to set back the instance
    /// </summary>
    public class SlowMethodContainerFix3 : SlowMethodContainerFix
    {
        private ISlowMethod OpProp
        {
            get { return operation; }
            set { operation = value; }
        }

        public SlowMethodContainerFix3(TimeSpan timeout) : base(timeout)
        {
        }

        public void FixUsingPopertyLinqExpression()
        {
            OpProp = Background<ISlowMethod>.StartMethod(() => OpProp, OpProp.Perform, methodInterception);
        }        
    }
}
