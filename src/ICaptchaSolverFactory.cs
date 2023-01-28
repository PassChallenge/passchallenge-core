using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core;

public interface ICaptchaSolverFactory
{
    public ICaptchaSolver<TCaptcha, TSolution> CreateSolver<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution;
    
    public bool CanCreateSolver<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution;
}