using System;
using System.Runtime.Serialization;

namespace BusinessLogic.Model
{
    [DataContract]
    internal class Message: IComparable<Message>
    {
        [DataMember]
        internal Guid Id { get; set; }

        [DataMember]
        internal string Text { get; set; }

        [DataMember]
        internal Person Person { get; set; }

        [DataMember]
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

        public int CompareTo(Message? other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            return SendingTime.CompareTo(other.SendingTime);
        }
    }
}
