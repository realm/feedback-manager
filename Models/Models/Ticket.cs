using System;
using System.Linq;
using Realms;

namespace Models
{
    public class Ticket : RealmObject
    {
        public string AuthorId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTimeOffset Date { get; set; }

        public float Score { get; set; }

        public string Tags { get; set; }

        public bool IsResolved { get; set; }

        [Backlink(nameof(Message.Ticket))]
        public IQueryable<Message> Messages { get; }
    }
}