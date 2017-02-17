using System;

namespace BackgroundTest.PinaColada
{
    class StepInterceptor : IPinaColadaStep
    {
        IPinaColadaStep instance;
        Action interceptionCallback;

        public StepInterceptor(IPinaColadaStep instance, Action interceptionCallback)
        {
            this.instance = instance;
            this.interceptionCallback = interceptionCallback;
        }

        public event StepCompleteEventHandler StepComplete
        {
            add
            {
                interceptionCallback();
                instance.StepComplete += value;
            }
            remove
            {
                interceptionCallback();
                instance.StepComplete -= value;
            }
        }

        public void Perform()
        {
            interceptionCallback();
            instance.Perform();
        }

        public int StepNumber
        {
            get
            {
                interceptionCallback();
                return instance.StepNumber;
            }
        }

        public bool Completed
        {
            get
            {
                interceptionCallback();
                return instance.Completed;
            }
        }
    }

}
