using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BackgroundTest.PinaColada
{
    [TestClass]
    public class PinaColadaTest
    {
        readonly TimeSpan timeout = TimeSpan.FromMilliseconds(300);

        [TestMethod]
        public void SynchroniclyBartender()
        {
            var pinaColadaCreator = new Bartender(timeout);
            TimeoutUtil.Test(pinaColadaCreator.MakeMePinaColada, timeout.Multiply(Bartender.TotalSteps), timeout.Half());
            TimeoutUtil.TestImmidiate(()=>Assert.IsTrue(pinaColadaCreator.DrinkReady));
            TimeoutUtil.TestImmidiate(()=>Assert.IsTrue(pinaColadaCreator.serve.Completed));
        }

        [TestMethod]
        public void ASynchroniclyBartender()
        {
            var pinaColadaCreator = new Bartender(timeout);
            TimeoutUtil.TestImmidiate(pinaColadaCreator.MakeMePinaColadaFast);
            TimeoutUtil.Test(() => Assert.IsTrue(pinaColadaCreator.DrinkReady),
               timeout.Multiply(Bartender.TotalStepsWhenParalleling));
            TimeoutUtil.TestImmidiate(() => Assert.IsTrue(pinaColadaCreator.serve.Completed));
        }

        [TestMethod]
        public void ASynchroniclyBartenderMemberTest()
        {
            var pinaColadaCreator = new Bartender(timeout);
            TimeoutUtil.TestImmidiate(pinaColadaCreator.MakeMePinaColadaFast);
            TimeoutUtil.Test(() => Assert.IsTrue(pinaColadaCreator.serve.Completed),
                timeout.Multiply(Bartender.TotalStepsWhenParalleling));
            TimeoutUtil.TestImmidiate(() => Assert.IsTrue(pinaColadaCreator.DrinkReady));
        }
    }
}
