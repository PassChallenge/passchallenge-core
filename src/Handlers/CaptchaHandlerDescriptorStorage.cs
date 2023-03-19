using System;
using System.Collections.Generic;
using System.Linq;
using PassChallenge.Core.Captcha;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Handlers;

internal class CaptchaHandlerDescriptorStorage : ICaptchaHandlerDescriptorAvailableStorage
{
    private readonly Dictionary<(Type captchaType, Type solutionType), Dictionary<string, CaptchaHandlerDescriptor>>
        _descriptors;

    public CaptchaHandlerDescriptorStorage(
        Dictionary<(Type captchaType, Type solutionType), Dictionary<string, CaptchaHandlerDescriptor>> descriptors)
    {
        _descriptors = descriptors ?? throw new ArgumentNullException(nameof(descriptors));
        Descriptors = _descriptors.SelectMany(x => x.Value).Select(x => x.Value).ToList();
    }

    public IReadOnlyCollection<CaptchaHandlerDescriptor> Descriptors { get; }


    public bool ContainsDescriptor<TCaptcha, TSolution>(string? descriptorName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        if (_descriptors.TryGetValue((typeof(TCaptcha), typeof(TSolution)), out var namedHandlers) == false)
            return false;

        return descriptorName == default ? namedHandlers.Count > 0 : namedHandlers.ContainsKey(descriptorName);
    }

    public string GetDefaultDescriptorName<TCaptcha, TSolution>() where TCaptcha : ICaptcha where TSolution : ISolution
    {
        if (ContainsDescriptor<TCaptcha, TSolution>() == false)
            throw new InvalidOperationException(
                $"Can't find descriptor for captcha type '{typeof(TCaptcha)}' and solution type '{typeof(TSolution)}'.");

        return _descriptors[(typeof(TCaptcha), typeof(TSolution))].First().Key;
    }

    public IReadOnlyCollection<CaptchaHandlerDescriptor> GetDescriptors<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        if (ContainsDescriptor<TCaptcha, TSolution>() == false)
            throw new InvalidOperationException(
                $"Can't find descriptor for captcha type '{typeof(TCaptcha)}' and solution type '{typeof(TSolution)}'.");

        return _descriptors[(typeof(TCaptcha), typeof(TSolution))].Values;
    }

    public CaptchaHandlerDescriptor GetDescriptor<TCaptcha, TSolution>(string? descriptorName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        if (ContainsDescriptor<TCaptcha, TSolution>(descriptorName))
            return descriptorName == default
                ? _descriptors[(typeof(TCaptcha), typeof(TSolution))].Values.First()
                : _descriptors[(typeof(TCaptcha), typeof(TSolution))][descriptorName];

        throw new InvalidOperationException(
            $"Can't find descriptor for captcha type '{typeof(TCaptcha)}' and solution type '{typeof(TSolution)}'" +
            (descriptorName == default ? "." : $" with name '{descriptorName}'."));
    }

    public bool IsAvailable<TCaptcha, TSolution>(string? handlerName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        return ContainsDescriptor<TCaptcha, TSolution>(handlerName);
    }
}