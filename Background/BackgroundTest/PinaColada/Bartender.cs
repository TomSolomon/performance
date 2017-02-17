using Performance;
using System;

namespace BackgroundTest.PinaColada
{
    class Bartender
    {
        public const int TotalSteps = 12;
        public const int TotalStepsWhenParalleling = 6;

        Func<IPinaColadaStep, Action, IPinaColadaStep> methodInterception = 
            (instance, callback) => new StepInterceptor(instance, callback);

        IPinaColadaStep openBlender;
        IPinaColadaStep openPiñaColadaMix;
        IPinaColadaStep putMixInBlender;
        IPinaColadaStep measureHalfCupWhiteRum;
        IPinaColadaStep pourInRum;
        IPinaColadaStep add2cupsIfIce;
        IPinaColadaStep closeBlender;
        IPinaColadaStep liquefyFor2Minutes;
        IPinaColadaStep openBlenderAgain;
        IPinaColadaStep getGlasses;
        IPinaColadaStep getPinkUmbrellas;

        // final step is set public to be used as part of a test
        public IPinaColadaStep serve;

        public Bartender(TimeSpan timeout)
        {
            openBlender = new PinaColadaStep(1, new IPinaColadaStep[] { }, timeout);
            openPiñaColadaMix = new PinaColadaStep(2, new IPinaColadaStep[] { }, timeout);
            putMixInBlender = new PinaColadaStep(3, new IPinaColadaStep[] { openBlender, openPiñaColadaMix }, timeout);
            measureHalfCupWhiteRum = new PinaColadaStep(4, new IPinaColadaStep[] { }, timeout);
            pourInRum = new PinaColadaStep(5, new IPinaColadaStep[] { measureHalfCupWhiteRum, openBlender }, timeout);
            add2cupsIfIce = new PinaColadaStep(6, new IPinaColadaStep[] { openBlender }, timeout);
            closeBlender = new PinaColadaStep(7, new IPinaColadaStep[] { pourInRum, add2cupsIfIce, putMixInBlender }, timeout);
            liquefyFor2Minutes = new PinaColadaStep(8, new IPinaColadaStep[] { closeBlender }, timeout);
            openBlenderAgain = new PinaColadaStep(9, new IPinaColadaStep[] { liquefyFor2Minutes }, timeout);
            getGlasses = new PinaColadaStep(10, new IPinaColadaStep[] { }, timeout);
            getPinkUmbrellas = new PinaColadaStep(11, new IPinaColadaStep[] { }, timeout);
            serve = new PinaColadaStep(12, new IPinaColadaStep[] { getGlasses, getPinkUmbrellas, openBlenderAgain }, timeout);
        }

        public bool DrinkReady { get { return serve.Completed; } }
       
        public void MakeMePinaColada()
        {
            openBlender.Perform();
            openPiñaColadaMix.Perform();
            putMixInBlender.Perform();
            measureHalfCupWhiteRum.Perform();
            pourInRum.Perform();
            add2cupsIfIce.Perform();
            closeBlender.Perform();
            liquefyFor2Minutes.Perform();
            openBlenderAgain.Perform();
            getGlasses.Perform();
            getPinkUmbrellas.Perform();
            serve.Perform();
        }     

        public void MakeMePinaColadaFast()
        {
            openBlender = Background<IPinaColadaStep>.StartMethod(() => openBlender, openBlender.Perform, methodInterception);
            openPiñaColadaMix = Background<IPinaColadaStep>.StartMethod(() => openPiñaColadaMix, openPiñaColadaMix.Perform, methodInterception);
            putMixInBlender = Background<IPinaColadaStep>.StartMethod(() => putMixInBlender, putMixInBlender.Perform, methodInterception);
            measureHalfCupWhiteRum = Background<IPinaColadaStep>.StartMethod(() => measureHalfCupWhiteRum, measureHalfCupWhiteRum.Perform, methodInterception);
            pourInRum = Background<IPinaColadaStep>.StartMethod(() => pourInRum, pourInRum.Perform, methodInterception);
            add2cupsIfIce = Background<IPinaColadaStep>.StartMethod(() => add2cupsIfIce, add2cupsIfIce.Perform, methodInterception);
            closeBlender = Background<IPinaColadaStep>.StartMethod(() => closeBlender, closeBlender.Perform, methodInterception);
            liquefyFor2Minutes = Background<IPinaColadaStep>.StartMethod(() => liquefyFor2Minutes, liquefyFor2Minutes.Perform, methodInterception);
            openBlenderAgain = Background<IPinaColadaStep>.StartMethod(() => openBlenderAgain, openBlenderAgain.Perform, methodInterception);
            getGlasses = Background<IPinaColadaStep>.StartMethod(() => getGlasses, getGlasses.Perform, methodInterception);
            getPinkUmbrellas = Background<IPinaColadaStep>.StartMethod(() => getPinkUmbrellas, getPinkUmbrellas.Perform, methodInterception);
            serve = Background<IPinaColadaStep>.StartMethod(() => serve, serve.Perform, methodInterception);
        }
    }
}
