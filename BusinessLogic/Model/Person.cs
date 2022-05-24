using System;
using System.Runtime.Serialization;

namespace BusinessLogic.Model
{
    [DataContract]
    internal class Person
    {
        [DataMember]
        internal Guid Id { get; set; }

        [DataMember]
        internal string Name { get; set; }

        internal Person()
        {
            Id = Guid.Empty;
            Name = "";
        }

        internal Person(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
