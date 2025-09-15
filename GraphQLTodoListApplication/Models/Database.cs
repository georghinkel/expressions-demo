using System.Collections.ObjectModel;
using NMF.Expressions.Linq;
using NMF.Expressions;

namespace GraphQLTodoListApplication.Models
{
    public partial class Database : ModelBase
    {
        public static readonly Database Instance = new Database();

        public ICollection<Project> Projects { get; } = new ObservableCollection<Project>();

        public INotifyEnumerable<Todo> TodosInOrder { get; }

        public Database()
        {
            var endOfTime = DateTime.MaxValue;
            ObservableExtensions.KeepOrder = true;

            TodosInOrder =
                     from p in Projects.WithUpdates()
                     where !p.IsOnHold
                     from t in p.Todos
                     where !t.IsDone
                     orderby t.Priority descending, t.Deadline ?? endOfTime ascending
                     select t;
        }

        private readonly Dictionary<Guid, ModelBase> _lookup = new Dictionary<Guid, ModelBase>();

        public void Register(ModelBase model)
        {
            _lookup[model.Id] = model;
        }

        public ModelBase Lookup(Guid id) => _lookup[id];
    }
}
