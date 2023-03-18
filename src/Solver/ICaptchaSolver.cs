using System.Threading;
using System.Threading.Tasks;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Solver;

public interface ICaptchaSolver<in TCaptcha, TSolution> where TCaptcha : ICaptcha where TSolution : ISolution
{
    public string? HandlerName { get; }

    Task<TSolution> Solve(TCaptcha captcha, CancellationToken cancellationToken = default);
}