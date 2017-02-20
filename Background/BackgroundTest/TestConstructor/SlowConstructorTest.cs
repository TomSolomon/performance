using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace BackgroundTest.TestConstructor
{  
    [TestClass]
    public class SlowConstructorTest
    {
        readonly TimeSpan timeout = TimeSpan.FromMilliseconds(300);

        [TestMethod]
        public void SlowCtoWithoutFix()
        {
            ISlowCtor slowCtor = null;
            TimeoutUtil.Test(() => slowCtor = new SlowCtor(timeout), timeout);
            Assert.IsNotNull(slowCtor);
            TimeoutUtil.TestImmidiate(slowCtor.InvokeObject);
        }

        [TestMethod]
        public void SlowCtorFieldLinqSetter()
        {
            var slowCtor = new SlowCtor1(timeout);
            TimeoutUtil.TestImmidiate(slowCtor.FieldLinqSetter);
            TimeoutUtil.Test(slowCtor.InvokeObject, timeout);
        }

        [TestMethod]
        public void SlowCtorFieldGenericActionSetter()
        {
            var slowCtor = new SlowCtor1(timeout);
            TimeoutUtil.TestImmidiate(slowCtor.GenericActionFieldSetter);
            TimeoutUtil.Test(slowCtor.InvokeObject, timeout);
        }
      
        [TestMethod]
        public void SlowCtorUserDefinedSetter()
        {
            var slowCtor = new SlowCtor3(timeout);
            TimeoutUtil.TestImmidiate(slowCtor.UserDefineSetter);
            TimeoutUtil.Test(slowCtor.InvokeObject, timeout);
        }

        [TestMethod]
        public void SlowCtorgenericActionPropertySetter()
        {
            var slowCtor = new SlowCtor2(timeout);
            TimeoutUtil.TestImmidiate(slowCtor.GenericActionPropertySetter);
            TimeoutUtil.Test(slowCtor.InvokeObject, timeout);
        }

        [TestMethod]
        public void SlowCtorgenericPropertyLinqSetter()
        {
            var slowCtor = new SlowCtor2(timeout);
            TimeoutUtil.TestImmidiate(slowCtor.PropertyLinqSetter);
            TimeoutUtil.Test(slowCtor.InvokeObject, timeout);
        }

        [TestMethod]
        public void SlowCtorMyModelTest()
        {
            MyModel slowCtor = null;            
            TimeoutUtil.Test(() => slowCtor = new MyModel(), Database.DatabaseContructionTime);
            Assert.IsNotNull(slowCtor);
            int? id = null;
            TimeoutUtil.TestImmidiate(() => id = slowCtor.GetID("bla bla bla"));
            Assert.IsTrue(id.HasValue && id == Database.DatabaseOnlyID);
        }

        [TestMethod]
        public void SlowCtorMyModelFixedTest()
        {
            MyModelFixed slowCtor = null;
            TimeoutUtil.TestImmidiate(() => slowCtor = new MyModelFixed());
            Assert.IsNotNull(slowCtor);

            Thread.Sleep(Database.DatabaseContructionTime);

            int? id = null;
            TimeoutUtil.TestImmidiate(() => id = slowCtor.GetID("bla bla bla"));
            Assert.IsTrue(id.HasValue && id == Database.DatabaseOnlyID);
        }
    }
}
