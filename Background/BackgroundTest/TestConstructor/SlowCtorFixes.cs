using Performance;
using Performance.Setters;
using System;

namespace BackgroundTest.TestConstructor
{    
    public class SlowCtor1
    {
        TimeSpan timeSpan;
        ISlowCtor objWithSlowCtor;

        public SlowCtor1(TimeSpan timeSpan)
        {
            this.timeSpan = timeSpan;
        }

        public void GenericActionFieldSetter()
        {
            objWithSlowCtor = Background<ISlowCtor>.StartCtor(() => new SlowCtor(timeSpan), 
                (newVal) => objWithSlowCtor = newVal,
                cb => new SlowCtorInterceptor(cb));
        }

        public void FieldLinqSetter()
        {
            objWithSlowCtor = Background<ISlowCtor>.StartCtor(() => new SlowCtor(timeSpan),
               () => objWithSlowCtor,
               cb => new SlowCtorInterceptor(cb));
        }

        public void InvokeObject()
        {
            objWithSlowCtor.InvokeObject();
        }
    }

    public class SlowCtor2
    {
        TimeSpan timeSpan;

        private ISlowCtor ObjWithSlowCtor { get; set; }

        public SlowCtor2(TimeSpan timeSpan)
        {
            this.timeSpan = timeSpan;
        }

        public void GenericActionPropertySetter()
        {
            ObjWithSlowCtor = Background<ISlowCtor>.StartCtor(() => new SlowCtor(timeSpan), 
               (newVal) => ObjWithSlowCtor = newVal,              
               cb => new SlowCtorInterceptor(cb));
        }

        public void PropertyLinqSetter()
        {
            ObjWithSlowCtor = Background<ISlowCtor>.StartCtor(() => new SlowCtor(timeSpan),
                () => ObjWithSlowCtor,
                cb => new SlowCtorInterceptor(cb));
        }

        public void InvokeObject()
        {
            ObjWithSlowCtor.InvokeObject();
        }
    }

    public class SlowCtor3: ISetter<ISlowCtor>
    {
        TimeSpan timeSpan;

        public void SetBack(ISlowCtor instance)
        {
            Value = instance;
        }

        private ISlowCtor Value { get; set; }

        public SlowCtor3(TimeSpan timeSpan)
        {
            this.timeSpan = timeSpan;
        }

        public void UserDefineSetter()
        {
            Value = Background<ISlowCtor>.StartCtor(() => new SlowCtor(timeSpan),
               this,
               cb => new SlowCtorInterceptor(cb));
        }

        public void InvokeObject()
        {
            Value.InvokeObject();
        }
    }
}
