using HotChocolate.AspNetCore;
using HotChocolateDictionaryBugReproduction.Domain;
using HotChocolateDictionaryBugReproduction.GraphQL;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddTransient<IMachineService, MachineService>();

builder.Services.AddMyGraphQL(builder.Environment.IsDevelopment())
    .InitializeOnStartup();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
// app.UseHttpStatusCodeExceptionMiddleware();
app.MapGraphQL()
    .AllowAnonymous()
    .WithOptions(new GraphQLServerOptions
    {
        AllowedGetOperations = AllowedGetOperations.QueryAndMutation,
        EnableGetRequests = true,
        Tool = { Enable = false }
    });

app.MapGraphQLWebSocket();

app.RunWithGraphQLCommands(args);
