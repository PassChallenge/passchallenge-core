using System.Threading.Tasks;

namespace PassChallenge.Core.Consumer;

public interface IConsumer
{
    string Name { get; }
    Task Start();
}