using CommunityToolkit.Mvvm.Input;
using NMF.Expressions.Linq;
using System.Collections.ObjectModel;

namespace MauiTodoListApplication.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ICollection<ProjectModel> Projects { get; } = new ObservableCollection<ProjectModel>();

        public IEnumerable<TodoModel> TodosInOrder { get; }

        private static readonly DateTime EndOfTime = DateTime.MaxValue;

        public MainWindowViewModel()
        {
            TodosInOrder =
                   (from p in Projects.WithUpdates()
                    where !p.IsOnHold
                    from t in p.Todos
                    where !t.IsDone
                    orderby t.Priority descending, t.Deadline ?? EndOfTime
                    select t).RestoreIndices();
        }

        [RelayCommand]
        private void AddProject()
        {
            Projects.Add(new ProjectModel { Name = $"Project {Projects.Count + 1}" });
        }
    }
}
