namespace HotChocolateDictionaryBugReproduction.GraphQL.Common;

public interface IUserError
{
    public string Message { get; set; }
}

public class UserError : InterfaceType<IUserError>
{
    protected override void Configure(IInterfaceTypeDescriptor<IUserError> descriptor)
    {
        descriptor.Name("UserError");
        descriptor.Description("A common interface that all user facing errors implement");
    }
}