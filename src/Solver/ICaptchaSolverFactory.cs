using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Solver;

public interface ICaptchaSolverFactory
{
    ICaptchaSolver<TCaptcha, TSolution> CreateSolver<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution;

    bool CanCreateSolver<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution;
}