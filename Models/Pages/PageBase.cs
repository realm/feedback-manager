using ViewModels;
using Xamarin.Forms;

namespace Pages
{
    public abstract class PageBase : ContentPage
    {
        public abstract ViewModelBase ViewModel { get; }

        protected PageBase()
        {
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.Initialize();
        }
    }
}