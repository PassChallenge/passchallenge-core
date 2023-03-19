using System.Threading.Tasks;

namespace KillDNS.ChallengeSolver.Core.Consumer;

public interface IConsumer
{
    Task Start();

    Task Stop();
}