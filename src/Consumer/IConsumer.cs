using System.Threading.Tasks;

namespace KillDNS.CaptchaSolver.Core.Consumer;

public interface IConsumer
{
    Task Start();
    
    Task Stop();
}