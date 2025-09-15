using System.Collections.Specialized;

namespace GraphQLTodoListApplication.Models
{
    public class TodoListChange
    {
        public TodoListChange() { }

        public TodoListChange(NotifyCollectionChangedEventArgs e)
        {
            Added = e.NewItems as IList<Todo> ?? e.NewItems?.OfType<Todo>().ToList();
            Removed = e.OldItems as IList<Todo> ?? e.OldItems?.OfType<Todo>().ToList();
            Action = e.Action;
            NewItemIndex = e.NewStartingIndex;
            OldItemIndex = e.OldStartingIndex;
        }

        public IList<Todo>? Added { get; init; }

        public IList<Todo>? Removed { get; init; }

        public NotifyCollectionChangedAction Action { get; init; }

        public int NewItemIndex { get; init; }

        public int OldItemIndex { get; init; }
    }
}
