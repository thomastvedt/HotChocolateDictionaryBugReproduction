using System.Runtime.CompilerServices;
using HotChocolate.Language;
using HotChocolateDictionaryBugReproduction.Domain;

namespace HotChocolateDictionaryBugReproduction.GraphQL.Common.Machines;

[ExtendObjectType(OperationType.Subscription)]
public class MachineSubscription
{
    public async IAsyncEnumerable<Machine> OnMachineUpdatedStream(
        Guid machineId,
        IServiceProvider serviceProvider,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var machineIdUpdatedObservable = serviceProvider.GetRequiredService<IMachineService>()
            .GetMachineUpdatedObservable();

        await foreach (var machineIdUpdated in machineIdUpdatedObservable.ToAsyncEnumerable().WithCancellation(cancellationToken))
        {
            if (machineIdUpdated != machineId) continue;

            using var scope = serviceProvider.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<MachineSubscription>>();

            Machine? machine;

            try
            {
                var machineService = scope.ServiceProvider.GetRequiredService<IMachineService>();
                machine = await machineService.GetMachine(machineIdUpdated);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Could not handle message channel message for machine id {MachineId}", machineIdUpdated);
                continue;
            }

            yield return machine;
        }
    }

    [Subscribe(With = nameof(OnMachineUpdatedStream))]
    public Machine OnMachineUpdated(Guid machineId, [EventMessage] Machine message) => message;
}

