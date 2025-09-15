using GraphQLTodoListApplication;
using GraphQLTodoListApplication.Models;
using HotChocolate.Subscriptions;

var builder = WebApplication.CreateBuilder(args);

builder.AddGraphQL()
    .AddInMemorySubscriptions()
    .AddQueryType<Queries>()
    .AddMutationType<Mutations>()
    .AddSubscriptionType<Subscriptions>();

var app = builder.Build();

var eventSender = app.Services.GetRequiredService<ITopicEventSender>();

Database.Instance.TodosInOrder.CollectionChanged += (_, e) =>
{
    _ = eventSender.SendAsync(nameof(Subscriptions.TodoPrioritiesChanged), new TodoListChange(e));
};

app.UseWebSockets();
app.MapGraphQL();

app.Run();
