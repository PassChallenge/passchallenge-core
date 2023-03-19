using PassChallenge.Core.Captcha;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Solver;

public interface IAvailableCaptchaAndSolutionStorage
{
    bool IsAvailable<TCaptcha, TSolution>(string? handlerName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution;
}