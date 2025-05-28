using HotChocolate.Execution.Configuration;
using HotChocolateDictionaryBugReproduction.GraphQL.Common;

namespace HotChocolateDictionaryBugReproduction.GraphQL;

public static class SchemaFactory
{
    public static IRequestExecutorBuilder AddMyGraphQL(
        this IServiceCollection services, bool isDevelopment)
    {
        var builder = services
            .AddGraphQLServer()
            // NOTE: Generated with source generators, <see cref="ModuleInfo"/>
            .AddMyTypes()
            .ModifyOptions(c =>
            {
                c.EnableOneOf = true;
                c.StrictValidation = false;
                c.DefaultQueryDependencyInjectionScope = DependencyInjectionScope.Request;
            })
            .AddInstrumentation()
            // NOTE: This will always allow introspection and schema download in dev mode!
            // NOTE: In production, this is handled by the IntrospectionMiddleware + ConnectAppHttpRequestInterceptor
            .DisableIntrospection(!isDevelopment)
            .AddQueryType(descriptor =>
            {
                descriptor.Name("Query");
                // This will make all queries require authorization by default, unless explicitly allowed using the [AllowAnonymous] attribute
                descriptor.Authorize();
            })
            .AddMutationType(descriptor =>
            {
                descriptor.Name("Mutation");
                descriptor.Authorize();
            })
            .AddSubscriptionType(descriptor =>
            {
                descriptor.Name("Subscription");
                descriptor.Authorize();
            })
            .BindRuntimeType<decimal, DecimalType>()
            .AddSorting()
            .AddGlobalObjectIdentification()
            .ModifyRequestOptions(config => { config.IncludeExceptionDetails = isDevelopment; })
            // .AddDiagnosticEventListener<ExecutionDiagnosticsListener>()
            .AddAuthorization()
            // .AddHttpRequestInterceptor<MyHttpRequestInterceptor>()
            // .AddSocketSessionInterceptor<AuthenticationSocketInterceptor>() // authenticate websocket requests + pre-parse ACL from token before each websocket request
            .ModifyPagingOptions(config =>
            {
                config.DefaultPageSize = 50;
                config.IncludeTotalCount = true;
                config.MaxPageSize = 100;
            })
            .ModifyCostOptions(config =>
            {
                config.MaxFieldCost = 2_500;
                config.MaxTypeCost = 2_500;
                config.EnforceCostLimits = true; // Set this to false if main menu query doesnt work
            })
            .AddErrorFilter<ErrorFilter>()
            .AddErrorInterfaceType<UserError>() // sets the shared UserError interface for user facing errors
            .AddMutationConventions(new MutationConventionOptions()
            {
                InputArgumentName = "input",
                ApplyToAllMutations = false // enable manually per mutation using attribute
            })
            .UseDefaultPipeline();
        // .UseRequest<CheckMutationAuthorizationMiddleware>();

        // if (apiSettings is { EnableTelemetry: true, TelemetryApiId: not null, TelemetryApiKey: not null, TelemetryStage: not null } && !isDevelopment)
        // {
        //     builder.AddNitro(config =>
        //     {
        //         config.ApiId = apiSettings.TelemetryApiId;
        //         config.ApiKey = apiSettings.TelemetryApiKey;
        //         config.Stage = apiSettings.TelemetryStage;
        //     });
        // }

        return builder;
    }
}