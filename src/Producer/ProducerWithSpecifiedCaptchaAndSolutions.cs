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
    private IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> _availableCaptchaAndSolutionTypes = null!;

    public virtual void SetAvailableCaptchaAndSolutionTypes(
        IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> availableCaptchaAndSolutionTypes)
    {
        _availableCaptchaAndSolutionTypes = availableCaptchaAndSolutionTypes;
    }

    public virtual bool CanProduce<TCaptcha, TSolution>() where TCaptcha : ICaptcha where TSolution : ISolution
    {
        if (_availableCaptchaAndSolutionTypes.Count == 0)
            return true;

        if (!_availableCaptchaAndSolutionTypes.ContainsKey(typeof(TCaptcha)))
            return false;

        if (_availableCaptchaAndSolutionTypes[typeof(TCaptcha)] is ISet<Type> settedValue &&
            settedValue.Contains(typeof(TSolution)))
            return true;

        return _availableCaptchaAndSolutionTypes[typeof(TCaptcha)].Contains(typeof(TSolution));
    }

    public abstract Task<TSolution> ProduceAndWaitSolution<TCaptcha, TSolution>(TCaptcha captcha,
        CancellationToken cancellationToken = default) where TCaptcha : ICaptcha where TSolution : ISolution;
}