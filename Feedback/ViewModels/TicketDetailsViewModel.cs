using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Realms;
using Realms.Sync;
using Xamarin.Forms;

namespace ViewModels
{
    public class TicketDetailsViewModel : ViewModelBase
    {
        private Ticket _ticket;
        private Realm _realm;
        private Message _message;
        private IEnumerable<Message> _messages;

        public Ticket Ticket
        {
            get => _ticket;
            set
            {
                Set(ref _ticket, value);
                Messages = value.Messages.OrderByDescending(m => m.Date);
                Message = new Message
                {
                    AuthorId = User.Current.Identity,
                    Ticket = value
                };
                _realm = value.Realm;
            }
        }

        public IEnumerable<Message> Messages
        {
            get => _messages;
            set => Set(ref _messages, value);
        }

        public Message Message
        {
            get => _message;
            set => Set(ref _message, value);
        }

        public Command SendCommand { get; }

        public TicketDetailsViewModel()
        {
            SendCommand = new Command(Send, () => !IsBusy);
        }

        private void Send()
        {
            PerformTask(() =>
            {
                Message.Date = DateTimeOffset.UtcNow;

                _realm.Write(() => _realm.Add(Message));

                Message = new Message
                {
                    AuthorId = User.Current.Identity,
                    Ticket = Ticket,
                    Text = string.Empty
                };

                return Task.CompletedTask;
            });
        }
    }
}
