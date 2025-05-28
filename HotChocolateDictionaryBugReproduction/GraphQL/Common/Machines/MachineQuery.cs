using HotChocolate.Language;
using HotChocolateDictionaryBugReproduction.Domain;

namespace HotChocolateDictionaryBugReproduction.GraphQL.Common.Machines;

[ExtendObjectType(OperationType.Query)]
public class MachineQuery
{
    public async ValueTask<Machine> GetMachine(
        Guid machineId,
        IMachineService machineService)
    {
        return await machineService.GetMachine(machineId);
    }
}
