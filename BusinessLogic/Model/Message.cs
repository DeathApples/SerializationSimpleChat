using System;
using System.Runtime.Serialization;

namespace BusinessLogic.Model
{
    [Serializable]
    public class Message : IComparable<Message>, ISerializable
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public Person Person { get; set; }

        public DateTime SendingTime { get; set; }

        public Message()
        {
            SendingTime = DateTime.UnixEpoch;
            Person = new Person();
            Id = Guid.Empty;
            Text = "";
        }

        public Message(Person person, string text)
        {
            SendingTime = DateTime.Now;
            Id = Guid.NewGuid();
            Person = person;
            Text = text;
        }

        public Message(SerializationInfo info, StreamingContext context)
        {
            object? temp = info.GetString("Id");
            Id = temp == null ? Guid.Empty : Guid.Parse((string)temp);

            Text = info.GetString("Text") ?? "";

            temp = info.GetValue("Person", typeof(Person));
            Person = temp == null ? new Person() : (Person)temp;

            temp = info.GetValue("Time", typeof(double));
            SendingTime = temp == null ? DateTime.UnixEpoch : DateTime.FromOADate((double)temp);
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", Id.ToString(), typeof(string));
            info.AddValue("Text", Text, typeof(string));
            info.AddValue("Person", Person, typeof(Person));
            info.AddValue("Time", SendingTime.ToOADate(), typeof(double));
        }
    }
}
