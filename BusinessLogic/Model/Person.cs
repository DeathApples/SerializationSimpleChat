using System;

namespace BusinessLogic.Model
{
    internal class Person
    {
        internal Guid Id { get; set; }
        internal string Name { get; set; }

        internal Person()
        {
            Id = Guid.Empty;
            Name = "";
        }

        internal Person(string name)
        {
            Id= Guid.NewGuid();
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
