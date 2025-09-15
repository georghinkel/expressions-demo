using GraphQLTodoListApplication.Models;

namespace GraphQLTodoListApplication
{
    public class Subscriptions
    {
        [Subscribe]
        [Topic(nameof(TodoPrioritiesChanged))]
        public TodoListChange TodoPrioritiesChanged([EventMessage] TodoListChange todoPriorities) => todoPriorities;
    }
}
