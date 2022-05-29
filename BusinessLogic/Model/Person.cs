using System;
using System.Runtime.Serialization;

namespace BusinessLogic.Model
{
    [Serializable]
    public class Person : ISerializable
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Person()
        {
            Id = Guid.Empty;
            Name = "";
        }

        public Person(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public Person(SerializationInfo info, StreamingContext context)
        {
            object? temp = info.GetString("Id");
            Id = temp == null ? Guid.Empty : Guid.Parse((string)temp);

            Name = info.GetString("Name") ?? "";
        }

        public override string ToString()
        {
            return Name;
        }

        public object GetDataObj()
        {
            return new { Id = Id, Name = Name };
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", Id.ToString(), typeof(string));
            info.AddValue("Name", Name, typeof(string));
        }
    }
}
