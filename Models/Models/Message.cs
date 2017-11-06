using System;
using Realms;

namespace Models
{
    public class Message : RealmObject
    {
        public string AuthorId { get; set; }

        public Ticket Ticket { get; set; }

        public DateTimeOffset Date { get; set; }

        public string Text { get; set; }
    }
}