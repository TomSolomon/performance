using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace BackgroundTest.PinaColada
{
    public delegate void StepCompleteEventHandler(PinaColadaStep step);

    public interface IPinaColadaStep
    {
        void Perform();

        int StepNumber { get; }

        bool Completed { get; }

        event StepCompleteEventHandler StepComplete;
    }

    public class PinaColadaStep : IPinaColadaStep
    {
        ConcurrentDictionary<int, IPinaColadaStep> preRequisitions;
        TimeSpan stepDuration;
        CountdownEvent e;

        public PinaColadaStep(int id, IEnumerable<IPinaColadaStep> preRequisitions, TimeSpan stepDuration)
        {
            StepNumber = id;
            this.preRequisitions =
                new ConcurrentDictionary<int, IPinaColadaStep>(preRequisitions.Select(step => new KeyValuePair<int, IPinaColadaStep>(step.StepNumber, step)));
            this.stepDuration = stepDuration;

            foreach (var preRequisition in preRequisitions)
            {
                preRequisition.StepComplete += PreRequisition_StepComplete;
            }

            e = new CountdownEvent(preRequisitions.Count());
        }

        public int StepNumber { get; set; }

        private void PreRequisition_StepComplete(PinaColadaStep step)
        {
            step.StepComplete -= PreRequisition_StepComplete;
            Debug.Assert(((IDictionary<int, IPinaColadaStep>)preRequisitions).Remove(step.StepNumber));
            e.Signal();
        }

        public void Perform()
        {
            e.Wait();
            Debug.Assert(!preRequisitions.Any());
            Debug.Assert(!Completed);
            Completed = true;
            Thread.Sleep(stepDuration);
            NotifyStepComplete();
        }

        public bool Completed { get; private set; }

        private void NotifyStepComplete()
        {
            StepComplete?.Invoke(this);
        }

        public event StepCompleteEventHandler StepComplete;
    }


}
