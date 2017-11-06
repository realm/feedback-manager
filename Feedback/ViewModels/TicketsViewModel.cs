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

        protected override bool IsPartial => true;

        public IEnumerable<Ticket> Tickets
        {
            get => _tickets;
            set => Set(ref _tickets, value);
        }

        public Command AddTicketCommand { get; }
        public Command LogoutCommand { get; }
        public Command<Ticket> ViewTicketCommand { get; }

        public TicketsViewModel()
        {
            AddTicketCommand = new Command(AddTicket, () => !IsBusy);
            LogoutCommand = new Command(Logout, () => !IsBusy);
            ViewTicketCommand = new Command<Ticket>(ViewTicket, _ => !IsBusy);
        }

        private void AddTicket()
        {
            PerformTask(async () =>
            {
                var ticket = await NavigationService.Prompt<NewTicketViewModel, Ticket>(vm => vm.Realm = _realm);
                await NavigationService.Navigate<TicketDetailsViewModel>(vm => vm.Ticket = ticket);
            });
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
            Tickets = (await _realm.SubscribeToObjectsAsync<Ticket>($"AuthorId == '{user.Identity}'"))
                            .OrderByDescending(t => t.Date);

            var foo = await _realm.SubscribeToObjectsAsync<Message>($"Ticket.AuthorId == '{user.Identity}'");
        }
    }
}