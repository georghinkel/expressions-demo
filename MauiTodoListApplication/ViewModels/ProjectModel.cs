using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTodoListApplication.ViewModels
{
    public partial class ProjectModel : ViewModelBase
    {
        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private bool isOnHold;

        public ICollection<TodoModel> Todos { get; } = new ObservableCollection<TodoModel>();

        [RelayCommand]
        private void AddTodo()
        {
            Todos.Add(new TodoModel { Text = "Insert Todo Title" });
        }
    }
}
