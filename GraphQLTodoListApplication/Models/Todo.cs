using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLTodoListApplication.Models
{
    public partial class Todo : ModelBase
    {
        private static TodoPriority[] _priorities = [TodoPriority.Low, TodoPriority.Medium, TodoPriority.High];

        public TodoPriority[] Priorities => _priorities;

        [ObservableProperty]
        private DateTime? deadline;

        [ObservableProperty]
        private bool isDone;

        [ObservableProperty]
        private TodoPriority priority;

        [ObservableProperty]
        private string? text;
    }
}
