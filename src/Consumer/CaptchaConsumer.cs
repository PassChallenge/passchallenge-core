using System.Threading;
using System.Threading.Tasks;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Handlers;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Consumer;

public class CaptchaConsumer<TCaptcha, TSolution> : IConsumer where TCaptcha : ICaptcha where TSolution : ISolution
{
    private readonly ICaptchaHandler<TCaptcha, TSolution> _captchaHandler;
    private readonly CancellationTokenSource _cts;
    private readonly TaskCompletionSource<int> _consumingStoppedTcs;

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

        throw new System.NotImplementedException();
    }

    public Task Stop()
    {
        throw new System.NotImplementedException();
    }
}