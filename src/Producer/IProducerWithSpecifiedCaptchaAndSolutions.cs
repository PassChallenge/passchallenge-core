using System;
using System.Collections.Generic;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Producer;

public interface IProducerWithSpecifiedCaptchaAndSolutions : IProducer
{
    void SetAvailableCaptchaAndSolutionTypes(
        IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> availableCaptchaAndSolutionTypes);

    bool CanProduce<TCaptcha, TSolution>() where TCaptcha : ICaptcha where TSolution : ISolution;
}