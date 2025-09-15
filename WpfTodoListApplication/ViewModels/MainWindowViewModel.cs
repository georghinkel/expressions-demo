using CommunityToolkit.Mvvm.Input;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NMF.Expressions.Linq;
using System;
using System.Collections.Specialized;
using NMF.Expressions;
using System.Diagnostics;
using System.Linq.Expressions;

namespace WpfTodoListApplication.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ICollection<ProjectModel> Projects { get; } = new ObservableCollection<ProjectModel>();

        public IEnumerable<TodoModel> TodosInOrder { get; }

        private static readonly DateTime EndOfTime = DateTime.MaxValue;

        public MainWindowViewModel()
        {
            ObservableExtensions.KeepOrder = true;

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
