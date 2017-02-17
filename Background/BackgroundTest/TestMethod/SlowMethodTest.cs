using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackgroundTest.TestMethod
{     
    [TestClass]
    public class SlowMethodTest
    {
        readonly TimeSpan timeout = TimeSpan.FromMilliseconds(300);

        [TestMethod]
        public void SlowMethodWithoutFix()
        {
            var problemContainer = new SlowMethodContainer(timeout);
            TimeoutUtil.Test(problemContainer.InvokeSlowMethod, timeout);
            TimeoutUtil.TestImmidiate(problemContainer.InvokeContainee);
        }

        [TestMethod]
        public void SlowMethodActiontTest()
        {
            var problemContainer = new SlowMethodContainerFix1(timeout);
            TimeoutUtil.TestImmidiate(problemContainer.FixUsingAction);
            TimeoutUtil.Test(problemContainer.InvokeContainee, timeout);
        }

        [TestMethod]
        public void SlowMethodGenericActionTest()
        {
            var problemContainer = new SlowMethodContainerFix1(timeout);
            TimeoutUtil.TestImmidiate(problemContainer.FixUsingGenericAction);
            TimeoutUtil.Test(problemContainer.InvokeContainee, timeout);            
        }

        [TestMethod]
        public void SlowMethodPropertyInLinqExpressionTest()
        {
            var problemContainer = new SlowMethodContainerFix3(timeout);
            TimeoutUtil.TestImmidiate(problemContainer.FixUsingPopertyLinqExpression);
            TimeoutUtil.Test(problemContainer.InvokeContainee, timeout);
        }

        [TestMethod]
        public void SlowMethodFieldInLinqExpressionTest()
        {
            var problemContainer = new SlowMethodContainerFix1(timeout);
            TimeoutUtil.TestImmidiate(problemContainer.FixUsingFieldLinqExpression);
            TimeoutUtil.Test(problemContainer.InvokeContainee, timeout);
        }

        [TestMethod]
        public void SlowMethodUsingUserDefinedSetter()
        {
            var problemContainer = new SlowMethodContainerFix2(timeout);
            TimeoutUtil.TestImmidiate(problemContainer.FixUsingUserDefinedSetter);
            TimeoutUtil.Test(problemContainer.InvokeContainee, timeout);
        }     
        
        [TestMethod]
        public void SlowMethodSleepAfterCallTest()
        {
            var problemContainer = new SlowMethodContainerFix2(timeout);
            TimeoutUtil.TestImmidiate(problemContainer.FixUsingUserDefinedSetter);
            Thread.Sleep(timeout);
            TimeoutUtil.TestImmidiate(problemContainer.InvokeContainee);
        }

        [TestMethod]
        public void SlowMethodDoubleInvocationTest()
        {
            var problemContainer = new SlowMethodContainerFix1(timeout);
            TimeoutUtil.TestImmidiate(problemContainer.FixUsingAction);
            TimeoutUtil.Test(problemContainer.InvokeContainee, timeout);
            TimeoutUtil.TestImmidiate(problemContainer.FixUsingAction);
            TimeoutUtil.Test(problemContainer.InvokeContainee, timeout);
        }

        [TestMethod]
        public void SlowMethodPartialDelayTest()
        {
            var problemContainer = new SlowMethodContainerFix1(timeout);
            TimeoutUtil.TestImmidiate(problemContainer.FixUsingAction);
            Thread.Sleep(timeout.Half());
            TimeoutUtil.Test(problemContainer.InvokeContainee, timeout.Half());
        }
    }
}
