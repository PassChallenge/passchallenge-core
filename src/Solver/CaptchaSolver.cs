using System;
using System.Threading;
using System.Threading.Tasks;
using PassChallenge.Core.Captcha;
using PassChallenge.Core.Producer;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Solver;

public class CaptchaSolver<TCaptcha, TSolution> : ICaptchaSolver<TCaptcha, TSolution>
    where TCaptcha : ICaptcha where TSolution : ISolution
{
    private readonly IProducer _producer;

    public CaptchaSolver(IProducer producer, string? handlerName = default)
    {
        _producer = producer ?? throw new ArgumentNullException(nameof(producer));
        HandlerName = handlerName;
    }

    public string? HandlerName { get; }

    public Task<TSolution> Solve(TCaptcha captcha, CancellationToken cancellationToken = default)
    {
        if (captcha == null)
            throw new ArgumentNullException(nameof(captcha));

        return _producer.ProduceAndWaitSolution<TCaptcha, TSolution>(captcha, HandlerName,
            cancellationToken);
    }
}