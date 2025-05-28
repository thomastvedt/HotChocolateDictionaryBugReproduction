using HotChocolate.Language;
using HotChocolate.Resolvers;
using HotChocolateDictionaryBugReproduction.Domain;

namespace HotChocolateDictionaryBugReproduction.GraphQL.Machines;

[ExtendObjectType(OperationType.Mutation)]
public class MachineMutation
{
    [UseMutationConvention]
    [HotChocolate.Authorization.Authorize(Policy = "IsAdmin")]
    public async Task<Machine> SetMachineServiceAgreement(
        Guid machineId,
        bool serviceAgreement,
        IMachineService machineService,
        IResolverContext context
    )
    {
        await machineService.SetMachineServiceAgreement(machineId, serviceAgreement);
        return await machineService.GetMachine(machineId);
    }

    // BUG: This adds "Task" to the schema in HC v15.1.4, but not in HC v15.1.3
    [UseMutationConvention]
    public async Task<Machine> DeleteMachineImage(
        Guid machineId,
        IMachineService machineService,
        IResolverContext context
    )
    {
        await machineService.DeleteImage(machineId);
        return await machineService.GetMachine(machineId);
    }
}