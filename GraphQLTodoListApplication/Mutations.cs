using GraphQLTodoListApplication.Models;

namespace GraphQLTodoListApplication
{
    public class Mutations
    {
        public Project AddProject(string projectName)
        {
            var project = new Project() { Name = projectName };
            Database.Instance.Register(project);
            Database.Instance.Projects.Add(project);
            return project;
        }

        public Project SetOnHold(Guid projectId, bool isOnHold)
        {
            var project = (Project)Database.Instance.Lookup(projectId);
            project.IsOnHold = isOnHold;
            return project;
        }

        public Todo AddTodo(Guid projectId, Todo todo)
        {
            var project = (Project)Database.Instance.Lookup(projectId);
            Database.Instance.Register(todo);
            project.Todos.Add(todo);
            return todo;
        }

        public Todo? RenameTodo(Guid todoId, string todoText)
        {
            return WithTodo(t => t.Text = todoText, todoId);
        }

        public Todo? MarkAsDone(Guid todoId)
        {
            return WithTodo(t => t.IsDone = true, todoId);
        }

        public Todo? ChangePriority(Guid todoId, TodoPriority priority)
        {
            return WithTodo(t => t.Priority = priority, todoId);
        }

        private Todo? WithTodo(Action<Todo> todoAction, Guid todoId)
        {
            if (Database.Instance.Lookup(todoId) is Todo todo)
            {
                todoAction(todo);
                return todo;
            }
            return null;
        }
    }
}
