using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLTodoListApplication.Models
{
    public partial class Project : ModelBase
    {
        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private bool isOnHold;

        public ICollection<Todo> Todos { get; } = new ObservableCollection<Todo>();
    }
}
