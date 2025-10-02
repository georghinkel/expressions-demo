using MauiTodoListApplication.ViewModels;

namespace MauiTodoListApplication
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainWindowViewModel();
        }
    }
}
