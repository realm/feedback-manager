using System;
using System.Threading.Tasks;
using Models;
using Realms;
using Realms.Sync;
using Services;
using Xamarin.Forms;

namespace ViewModels
{
    public class NewTicketViewModel : ViewModelBase, IPromptable<Ticket>
    {
        #region Promptable

        public Action<Ticket> Success { get; set; }

        public Action Cancel { get; set; }

        public Action<Exception> Error { get; set; }

        #endregion

        public Realm Realm { get; set; }

        public Ticket Ticket { get; } = new Ticket
        {
            AuthorId = User.Current.Identity,
            Score = 0.5f
        };

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public NewTicketViewModel()
        {
            SaveCommand = new Command(Save, () => !IsBusy);
            CancelCommand = new Command(() => Cancel(), () => !IsBusy);
        }

        private void Save()
        {
            PerformTask(() =>
            {
                Ticket.Date = DateTimeOffset.UtcNow;
                Realm.Write(() =>
                {
                    Realm.Add(new Message
                    {
                        Ticket = Ticket,
                        AuthorId = Ticket.AuthorId,
                        Date = Ticket.Date,
                        Text = Ticket.Description
                    });
                });
                Success(Ticket);

                return Task.CompletedTask;
            });
        }
    }
}
