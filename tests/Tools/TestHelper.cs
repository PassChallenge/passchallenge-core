using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace PassChallenge.Core.Tests.Tools;

public static class TestHelper
{
    public static IList<ServiceDescriptor> MakeEnumerableDescriptors(Mock<IServiceCollection> mock)
    {
        List<ServiceDescriptor> descriptors = new();
        mock.As<IEnumerable>().Setup(r => r.GetEnumerator()).Returns(() => descriptors.GetEnumerator());
        mock.Setup(x => x.Add(Capture.In(descriptors)));
        return descriptors;
    }
}