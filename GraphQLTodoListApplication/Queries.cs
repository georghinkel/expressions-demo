using GraphQLTodoListApplication.Models;

namespace GraphQLTodoListApplication
{
    public class Queries
    {
        public IEnumerable<Project> Projects() => Database.Instance.Projects;

        public IEnumerable<Todo> TodosInOrder() => Database.Instance.TodosInOrder;
    }
}
