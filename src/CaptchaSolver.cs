using System;
using System.Threading;
using System.Threading.Tasks;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Producer;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core;

public class CaptchaSolver<TCaptcha, TSolution> : ICaptchaSolver<TCaptcha, TSolution>
    where TCaptcha : ICaptcha where TSolution : ISolution
{
    private readonly IProducer _producer;

    public CaptchaSolver(IProducer producer)
    {
        _producer = producer ?? throw new ArgumentNullException(nameof(producer));
    }

    public Task<TSolution> Solve(TCaptcha captcha, CancellationToken cancellationToken = default)
    {
        if (captcha == null)
            throw new ArgumentNullException(nameof(captcha));

        return _producer.ProduceAndWaitSolution<TCaptcha, TSolution>(captcha, cancellationToken);
    }
}