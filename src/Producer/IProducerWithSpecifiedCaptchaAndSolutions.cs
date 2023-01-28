using System;
using System.Collections.Generic;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Producer;

public interface IProducerWithSpecifiedCaptchaAndSolutions : IProducer
{
    public void SetAvailableCaptchaAndSolutionTypes(
        IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> availableCaptchaAndSolutionTypes);

    public bool CanProduce<TCaptcha, TSolution>() where TCaptcha : ICaptcha where TSolution : ISolution;
}