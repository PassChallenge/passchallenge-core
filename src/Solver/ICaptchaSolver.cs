using System.Threading;
using System.Threading.Tasks;
using PassChallenge.Core.Captcha;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Solver;

public interface ICaptchaSolver<in TCaptcha, TSolution> where TCaptcha : ICaptcha where TSolution : ISolution
{
    public string? HandlerName { get; }

    Task<TSolution> Solve(TCaptcha captcha, CancellationToken cancellationToken = default);
}