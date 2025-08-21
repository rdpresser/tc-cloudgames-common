using Wolverine;
using Wolverine.Runtime;

namespace TC.CloudGames.SharedKernel.Infrastructure.Messaging;

public interface IEnvelopeCustomizer
{
    void Customize(Envelope envelope, IWolverineRuntime runtime);
}
