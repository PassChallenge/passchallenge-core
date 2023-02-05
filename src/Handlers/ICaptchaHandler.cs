using System.Threading;
using System.Threading.Tasks;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Handlers;

public interface ICaptchaHandler<in TCaptcha, TSolution> where TCaptcha : ICaptcha
    where TSolution : ISolution
{
    Task<TSolution> Handle(TCaptcha captcha, CancellationToken cancellationToken = default);
}