using System;

namespace BusinessLogic.Model
{
    internal class Message
    {
        internal Guid Id { get; set; }
        internal string Text { get; set; }
        internal Person Person { get; set; }
        public DateTime SendingTime { get; set; }

        internal Message()
        {
            SendingTime = DateTime.UnixEpoch;
            Person = new Person();
            Id = Guid.Empty;
            Text = "";
        }

        internal Message(Person person, string text)
        {
            SendingTime = DateTime.Now;
            Id = Guid.NewGuid();
            Person = person;
            Text = text;
        }

        public override string ToString()
        {
            return $"{SendingTime:T} [{Person}]: {Text}";
        }
    }
}
