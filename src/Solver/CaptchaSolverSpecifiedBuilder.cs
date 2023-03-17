using System;
using KillDNS.CaptchaSolver.Core.Producer;

namespace KillDNS.CaptchaSolver.Core.Solver;

public class CaptchaSolverSpecifiedBuilder<TProducer> : CaptchaSolverBuilder<TProducer>
    where TProducer : IProducerWithSpecifiedCaptchaAndSolutions
{
    public AvailableCaptchaAndSolutionStorageBuilder AvailableCaptchaAndSolutionStorageBuilder { get; } = new();

    protected override TProducer InternalBuild(IServiceProvider provider)
    {
        TProducer producer = base.InternalBuild(provider);
        producer.SetAvailableCaptchaAndSolutionStorage(AvailableCaptchaAndSolutionStorageBuilder.Build());
        return producer;
    }
}