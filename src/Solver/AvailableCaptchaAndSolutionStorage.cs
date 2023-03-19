using System;
using System.Collections.Generic;
using PassChallenge.Core.Captcha;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Solver;

public class AvailableCaptchaAndSolutionStorage : IAvailableCaptchaAndSolutionStorage
{
    private readonly Dictionary<Type, Dictionary<Type, HashSet<string>>> _availableCaptchaAndSolutions;

    internal AvailableCaptchaAndSolutionStorage(
        Dictionary<Type, Dictionary<Type, HashSet<string>>> availableCaptchaAndSolutions)
    {
        _availableCaptchaAndSolutions = availableCaptchaAndSolutions ??
                                        throw new ArgumentNullException(nameof(availableCaptchaAndSolutions));
    }

    public bool IsAvailable<TCaptcha, TSolution>(string? handlerName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        return _availableCaptchaAndSolutions.TryGetValue(typeof(TCaptcha),
                   out Dictionary<Type, HashSet<string>> solutionTypes) &&
               (handlerName == default ||
                solutionTypes.TryGetValue(typeof(TSolution), out HashSet<string> handlerNames) &&
                handlerNames.Contains(handlerName));
    }
}