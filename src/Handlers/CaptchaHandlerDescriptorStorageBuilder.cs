using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Handlers;

public class CaptchaHandlerDescriptorStorageBuilder
{
    private readonly Dictionary<(Type captchaType, Type solutionType), Dictionary<string, CaptchaHandlerDescriptor>>
        _descriptors = new();

    public CaptchaHandlerDescriptorStorageBuilder AddCaptchaHandler<TCaptcha, TSolution, THandler>(
        string? handlerName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution where THandler : ICaptchaHandler<TCaptcha, TSolution>
    {
        AddHandlerDescriptor(CaptchaHandlerDescriptor.Create<TCaptcha, TSolution, THandler>(handlerName));
        return this;
    }

    public CaptchaHandlerDescriptorStorageBuilder AddCaptchaHandler<TCaptcha, TSolution, THandler>(
        Func<IServiceProvider, THandler> factory,
        string? handlerName = default) where TCaptcha : ICaptcha
        where TSolution : ISolution
        where THandler : ICaptchaHandler<TCaptcha, TSolution>
    {
        AddHandlerDescriptor(CaptchaHandlerDescriptor.Create<TCaptcha, TSolution, THandler>(factory, handlerName));
        return this;
    }

    public CaptchaHandlerDescriptorStorageBuilder AddCaptchaHandler<TCaptcha, TSolution>(
        Func<IServiceProvider, TCaptcha, Task<TSolution>> func,
        string? handlerName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        AddHandlerDescriptor(CaptchaHandlerDescriptor.Create(func, handlerName));
        return this;
    }

    public ICaptchaHandlerDescriptorAvailableStorage Build()
    {
        return new CaptchaHandlerDescriptorStorage(_descriptors);
    }

    private void AddHandlerDescriptor(CaptchaHandlerDescriptor descriptor)
    {
        if (_descriptors.ContainsKey((descriptor.CaptchaType, descriptor.SolutionType)) == false)
            _descriptors.Add((descriptor.CaptchaType, descriptor.SolutionType),
                new Dictionary<string, CaptchaHandlerDescriptor>());

        Dictionary<string, CaptchaHandlerDescriptor> namedDescriptors =
            _descriptors[(descriptor.CaptchaType, descriptor.SolutionType)];

        CaptchaHandlerDescriptor newDescriptor = descriptor.HandlerName != default
            ? descriptor
            : descriptor.CloneWithNewName(GetNewHandlerIdentifier(namedDescriptors.Keys, descriptor));

        if (namedDescriptors.ContainsKey(newDescriptor.HandlerName!))
            throw new InvalidOperationException(
                $"Descriptor with captcha '{newDescriptor.CaptchaType}' and solution '{newDescriptor.SolutionType}' and handler name '{newDescriptor.HandlerName}' already added.");

        namedDescriptors.Add(newDescriptor.HandlerName!, newDescriptor);
    }

    private string GetNewHandlerIdentifier(IReadOnlyCollection<string> allHandlerNames,
        CaptchaHandlerDescriptor descriptor)
    {
        string baseHandlerName = $"{descriptor.CaptchaType.Name}-{descriptor.SolutionType.Name}-".ToLower();
        for (int i = 0;; i++)
        {
            string finalHandlerName = baseHandlerName + i;

            if (allHandlerNames.Contains(finalHandlerName) == false)
                return finalHandlerName;
        }
    }
}