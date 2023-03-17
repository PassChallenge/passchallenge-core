using System;
using System.Threading;
using System.Threading.Tasks;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Handlers;

internal class FixedCaptchaHandler<TCaptcha, TSolution> : ICaptchaHandler<TCaptcha, TSolution>
    where TCaptcha : ICaptcha
    where TSolution : ISolution
{
    private readonly Func<IServiceProvider, TCaptcha, Task<TSolution>> _resolveFunc;
    private readonly IServiceProvider _serviceProvider;

    public FixedCaptchaHandler(IServiceProvider serviceProvider,
        Func<IServiceProvider, TCaptcha, Task<TSolution>> resolveFunc)
    {
        _serviceProvider = serviceProvider;
        _resolveFunc = resolveFunc ?? throw new ArgumentNullException(nameof(resolveFunc));
    }

    public Task<TSolution> Handle(TCaptcha captcha, CancellationToken cancellationToken = default)
    {
        return _resolveFunc.Invoke(_serviceProvider, captcha);
    }
}