using System.Threading;
using System.Threading.Tasks;
using PassChallenge.Core.Captcha;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Handlers;

public interface ICaptchaHandler<in TCaptcha, TSolution> where TCaptcha : ICaptcha
    where TSolution : ISolution
{
    Task<TSolution> Handle(TCaptcha captcha, CancellationToken cancellationToken = default);
}