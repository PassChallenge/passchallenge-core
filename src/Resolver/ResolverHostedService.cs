using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace KillDNS.CaptchaSolver.Core.Resolver;

//TODO: WIP
public class ResolverHostedService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}