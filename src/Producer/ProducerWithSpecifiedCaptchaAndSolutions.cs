using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Producer;

public abstract class ProducerWithSpecifiedCaptchaAndSolutions : IProducerWithSpecifiedCaptchaAndSolutions
{
    private IReadOnlyDictionary<Type, IReadOnlyCollection<Type>>? _availableCaptchaAndSolutionTypes;

    public virtual void SetAvailableCaptchaAndSolutionTypes(
        IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> availableCaptchaAndSolutionTypes)
    {
        _availableCaptchaAndSolutionTypes = availableCaptchaAndSolutionTypes ??
                                            throw new ArgumentNullException(nameof(availableCaptchaAndSolutionTypes));
    }

    public virtual bool CanProduce<TCaptcha, TSolution>() where TCaptcha : ICaptcha where TSolution : ISolution
    {
        return (_availableCaptchaAndSolutionTypes?.TryGetValue(typeof(TCaptcha),
            out IReadOnlyCollection<Type> solutionTypes) ?? false) && solutionTypes.Contains(typeof(TSolution));
    }

    public abstract Task<TSolution> ProduceAndWaitSolution<TCaptcha, TSolution>(TCaptcha captcha,
        CancellationToken cancellationToken = default) where TCaptcha : ICaptcha where TSolution : ISolution;
}