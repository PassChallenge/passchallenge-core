namespace PassChallenge.Core.Handlers;

public interface IBuilderWithCaptchaHandlers
{
    CaptchaHandlerDescriptorStorageBuilder DescriptorStorageBuilder { get; }
}