namespace HotChocolateDictionaryBugReproduction.Domain;

public interface IMachineService
{
    Task<Machine> GetMachine(Guid machineId);
    Task SetMachineServiceAgreement(Guid machineId, bool serviceAgreement);
    IObservable<Guid> GetMachineUpdatedObservable();
    Task DeleteImage(Guid machineId);
}

public record Machine(Guid Id, string Name, bool ServiceAgreement);

public class MachineService : IMachineService
{
    private readonly List<Machine> _machines = new()
    {
        new Machine(Guid.NewGuid(), "Machine 1", false),
        new Machine(Guid.NewGuid(), "Machine 2", true),
        new Machine(Guid.NewGuid(), "Machine 3", false),
        new Machine(Guid.NewGuid(), "Machine 4", true),
        new Machine(Guid.NewGuid(), "Machine 5", false),
        new Machine(Guid.NewGuid(), "Machine 6", true),
        new Machine(Guid.NewGuid(), "Machine 7", false),
        new Machine(Guid.NewGuid(), "Machine 8", true),
        new Machine(Guid.NewGuid(), "Machine 9", false),
        new Machine(Guid.NewGuid(), "Machine 10", true)
    };

    public async Task<Machine> GetMachine(Guid machineId)
    {
        await Task.Delay(100); // Simulate async delay
        return _machines.FirstOrDefault(m => m.Id == machineId) ??
               throw new Exception($"Machine with id {machineId} not found");
    }

    public async Task SetMachineServiceAgreement(Guid machineId, bool serviceAgreement)
    {
        await Task.Delay(100); // Simulate async delay
        var machineIndex = _machines.FindIndex(m => m.Id == machineId);
        if (machineIndex == -1)
            throw new Exception($"Machine with id {machineId} not found");

        _machines[machineIndex] = _machines[machineIndex] with { ServiceAgreement = serviceAgreement };
    }

    public IObservable<Guid> GetMachineUpdatedObservable()
    {
        return System.Reactive.Linq.Observable.Create<Guid>(observer =>
        {
            var random = new Random();
            var cancellationTokenSource = new CancellationTokenSource();

            Task.Run(async () =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    var randomMachine = _machines[random.Next(_machines.Count)];
                    observer.OnNext(randomMachine.Id);
                    await Task.Delay(TimeSpan.FromSeconds(random.Next(10, 30)), cancellationTokenSource.Token);
                }
            }, cancellationTokenSource.Token);

            return () =>
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            };
        });
    }

    public async Task DeleteImage(Guid machineId)
    {
        await Task.Delay(200);
    }
}