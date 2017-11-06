using Converters;
using Services;
using ViewModels;
using Xamarin.Forms;

namespace Feedback
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var navigationService = DependencyService.Get<INavigationService>(DependencyFetchTarget.GlobalInstance);
            navigationService.SetMainPage<TicketsViewModel>();

            Resources = new ResourceDictionary
            {
                ["AuthorToMarginConverter"] = new AuthorToMarginConverter(),
                ["AuthorToColorConverter"] = new AuthorToColorConverter(),
                ["IsResolvedToAlphaConverter"] = new IsResolvedToAlphaConverter(),
            };
        }
    }
}