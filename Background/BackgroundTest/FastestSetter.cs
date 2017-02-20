using BackgroundTest.TestMethod;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Performance;
using Performance.Setters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BackgroundTest
{
    public class EmptySlowMethod : ISlowMethod
    {
        public void Perform()
        {
        }

        public void InvokeObject()
        { }
    }    

    class u
    {
        public static T Make<T,S>(S s)where T:new() { return new T(); }
    }

    class FastestAllFixes : ISetter<ISlowMethod>
    {
        Func<ISlowMethod, Action, ISlowMethod> methodInterception = 
            (instance, callback) => new SlowMethodInterceptor(instance, callback);

        ISlowMethod operation;

        public FastestAllFixes()
        {
            operation = new EmptySlowMethod();
        }

        ISlowMethod Operation
        {
            get { return operation; }
            set { operation = value; }
        }

        public void FixUsingAction()
        {
            // for this example we will imitate a creation of SlowOperation localy and passing a reference to the newly created one
            // and using it to set back to new value
            ISlowMethod localRef = operation;//new SlowOperation(timeout);
            operation = Background<ISlowMethod>.StartMethod((Action)(() => operation = localRef),
                localRef.Perform, methodInterception);
        }

        public void FixUsingFieldLinqExpression()
        {
            operation = Background<ISlowMethod>.StartMethod(operation.Perform, () => operation, methodInterception);
        }

        public void FixUsingGenericAction()
        {
            operation = Background<ISlowMethod>.StartMethod(operation.Perform, (newValue) => operation = newValue, methodInterception);
        }

        public void FixUsingUserDefinedSetter()
        {
            operation = Background<ISlowMethod>.StartMethod(operation.Perform, this, methodInterception);
        }

        public void SetBack(ISlowMethod instance)
        {
            operation = instance;
        }

        public void FixUsingPopertyLinqExpression()
        {
            Operation = Background<ISlowMethod>.StartMethod(Operation.Perform, () => Operation,  methodInterception);
        }

        public void InvokeContainee()
        {
            Operation.InvokeObject();
        }
    }




    [TestClass]
    public class FastestSetter
    {
        const int tries = 1000000;
        SortedList<long, string> results = new SortedList<long, string>();

        public void SlowMethodActiontTest()
        {
            var problemContainer = new FastestAllFixes();

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < tries; i++)
            {
                problemContainer.FixUsingAction();
                problemContainer.InvokeContainee();
            }
            sw.Stop();
            results.Add(sw.ElapsedMilliseconds, "Action Setter");
        }

        public void SlowMethodGenericActionTest()
        {
            var problemContainer = new FastestAllFixes();

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < tries; i++)
            {
                problemContainer.FixUsingGenericAction();
                problemContainer.InvokeContainee();
            }
            sw.Stop();
            results.Add(sw.ElapsedMilliseconds, "Generic Action Setter");
        }

        public void SlowMethodPropertyInLinqExpressionTest()
        {
            var problemContainer = new FastestAllFixes();

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < tries; i++)
            {
                problemContainer.FixUsingPopertyLinqExpression();
                problemContainer.InvokeContainee();
            }
            sw.Stop();
            results.Add(sw.ElapsedMilliseconds, "linq property Setter");
        }

        public void SlowMethodFieldInLinqExpressionTest()
        {
            var problemContainer = new FastestAllFixes();

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < tries; i++)
            {
                problemContainer.FixUsingFieldLinqExpression();
                problemContainer.InvokeContainee();
            }
            sw.Stop();
            results.Add(sw.ElapsedMilliseconds, "linq field Setter");
        }

        public void SlowMethodUsingUserDefinedSetter()
        {
            var problemContainer = new FastestAllFixes();

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < tries; i++)
            {
                problemContainer.FixUsingUserDefinedSetter();
                problemContainer.InvokeContainee();
            }
            sw.Stop();
            results.Add(sw.ElapsedMilliseconds, "user defined Setter");
        }

       // [TestMethod]
        public void TestSpeed()
        {
            SlowMethodFieldInLinqExpressionTest();
            SlowMethodActiontTest();
            SlowMethodUsingUserDefinedSetter();
            SlowMethodGenericActionTest();
            SlowMethodPropertyInLinqExpressionTest();
            string resultStr = string.Join(Environment.NewLine, results.Select((pair) => pair.Value + " Took " + pair.Key + "[MS]"));
            resultStr += Environment.NewLine + results.First().Value + " is the fastests!!!";
            throw new Exception(resultStr);
        }
    }
}
