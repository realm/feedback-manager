using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Realms.Sync;
using Xamarin.Forms;

namespace ViewModels
{
    public class TicketsViewModel : MainViewModelBase
    {
        private IEnumerable<Ticket> _tickets;
        public IEnumerable<Ticket> Tickets
        {
            get => _tickets;
            set => Set(ref _tickets, value);
        }

        public Command LogoutCommand { get; }
        public Command<Ticket> ViewTicketCommand { get; }

        public TicketsViewModel()
        {
            LogoutCommand = new Command(Logout, () => !IsBusy);
            ViewTicketCommand = new Command<Ticket>(ViewTicket, _ => !IsBusy);
        }

        private void ViewTicket(Ticket ticket)
        {
            PerformTask(() =>
            {
                return NavigationService.Navigate<TicketDetailsViewModel>(vm => vm.Ticket = ticket);
            });
        }

        private void Logout()
        {
            PerformTask(async () =>
            {
                await User.Current?.LogOutAsync();
                NavigationService.SetMainPage<TicketsViewModel>();
            }, progressMessage: "Logging out...");
        }

        protected override async Task CompleteInitializationAsync(User user)
        {
            await _realm.GetSession().WaitForDownloadAsync();

            Tickets = _realm.All<Ticket>().OrderBy(t => t.IsResolved).ThenBy(t => t.Score);
        }
    }
}