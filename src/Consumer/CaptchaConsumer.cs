using System;
using System.Threading;
using System.Threading.Tasks;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Handlers;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Consumer;

//TODO: WIP
public class CaptchaConsumer<TCaptcha, TSolution> : IConsumer where TCaptcha : ICaptcha where TSolution : ISolution
{
    private readonly ICaptchaHandler<TCaptcha, TSolution> _captchaHandler;
    private readonly TaskCompletionSource<int> _consumingStoppedTcs;
    private readonly CancellationTokenSource _cts;

    public CaptchaConsumer(IConsumer d, ICaptchaHandler<TCaptcha, TSolution> captchaHandler)
    {
        _captchaHandler = captchaHandler;
        _cts = new CancellationTokenSource();
        _consumingStoppedTcs = new TaskCompletionSource<int>();
    }

    public async Task Start()
    {
        await Task.Yield();
        CancellationToken cancellationToken = _cts.Token;

        while (!cancellationToken.IsCancellationRequested)
        {
        }

        throw new NotImplementedException();
    }

    public Task Stop()
    {
        throw new NotImplementedException();
    }
}