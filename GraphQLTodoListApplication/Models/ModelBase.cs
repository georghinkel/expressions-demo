using CommunityToolkit.Mvvm.ComponentModel;

namespace GraphQLTodoListApplication.Models
{
    public abstract class ModelBase : ObservableObject
    {
        public Guid Id { get; }

        protected ModelBase() { Id = Guid.NewGuid(); }
    }
}
