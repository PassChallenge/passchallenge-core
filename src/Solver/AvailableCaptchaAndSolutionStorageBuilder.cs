using System;
using System.Collections.Generic;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Handlers;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Solver;

public class AvailableCaptchaAndSolutionStorageBuilder
{
    private readonly Dictionary<Type, Dictionary<Type, HashSet<string>>>
        _availableCaptchaAndSolutionTypes = new();

    private IAvailableCaptchaAndSolutionStorage? _availableCaptchaAndSolutionStorage;

    public AvailableCaptchaAndSolutionStorageBuilder AddSupportCaptchaAndSolution<TCaptcha, TSolution>(
        string? handlerName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        AddSupportCaptchaAndSolution(typeof(TCaptcha), typeof(TSolution), handlerName);
        return this;
    }

    public AvailableCaptchaAndSolutionStorageBuilder AddSupportCaptchaAndSolution(CaptchaHandlerDescriptor descriptor)
    {
        if (descriptor == null)
            throw new ArgumentNullException(nameof(descriptor));

        AddSupportCaptchaAndSolution(descriptor.CaptchaType, descriptor.SolutionType, descriptor.HandlerName);
        return this;
    }

    public void SetStorage(IAvailableCaptchaAndSolutionStorage storage)
    {
        _availableCaptchaAndSolutionStorage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    public IAvailableCaptchaAndSolutionStorage Build()
    {
        return _availableCaptchaAndSolutionStorage ??
               new AvailableCaptchaAndSolutionStorage(_availableCaptchaAndSolutionTypes);
    }

    private void AddSupportCaptchaAndSolution(Type captchaType, Type solutionType,
        string? handlerName = default)
    {
        if (!_availableCaptchaAndSolutionTypes.ContainsKey(captchaType))
        {
            _availableCaptchaAndSolutionTypes.Add(captchaType, new Dictionary<Type, HashSet<string>>());
            _availableCaptchaAndSolutionTypes[captchaType].Add(solutionType, new HashSet<string>());
        }

        if (handlerName == default)
            return;

        if (_availableCaptchaAndSolutionTypes[captchaType][solutionType].Contains(handlerName))
            throw new InvalidOperationException(
                $"Captcha '{captchaType}' and solution '{solutionType}' with handler name '{handlerName}' already added.");

        _availableCaptchaAndSolutionTypes[captchaType][solutionType].Add(handlerName);
    }
}