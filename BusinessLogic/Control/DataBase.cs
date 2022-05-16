using System;
using System.Linq;
using BusinessLogic.Model;
using System.Collections.Generic;

namespace BusinessLogic.Control
{
    public static class DataBase
    {
        public static event Action ChangeDataBase;

        internal static Person Current { get; private set; }
        internal static List<Person> Persons { get; private set; }
        internal static List<Message> Messages { get; private set; }

        public static string CurrentName { get => Current.Name; }
        public static int CountPerson { get => Persons?.Count ?? 0; }
        public static int CountMessage { get => Messages?.Count ?? 0; }

        static DataBase()
        {
            Persons = new List<Person>();
            Messages = new List<Message>();
            Current = new Person("New User");

            Persons.Add(Current);
        }

        public static void EditCurrentPerson(string name) => Current.Name = name;

        public static void SendMessage(string text)
        {
            Messages?.Add(new Message(Current, text));
            Synchronization.Serialize();
        }

        public static string GetAllPersons()
        {
            string info = "";

            foreach (var person in Persons)
            {
                info += $"{person}\n";
            }

            return info;
        }

        public static string GetAllMessages()
        {
            string info = "";

            foreach (var message in Messages)
            {
                info += $"{message}\n";
            }

            return info;
        }

        internal static void UpdateDataBase(List<Person> persons, List<Message> messages)
        {
            int countPerson = CountPerson, countMessage = CountMessage;

            foreach (var person in persons)
            {
                if (!(Persons?.Any(prototype => prototype.Id == person.Id) ?? false))
                {
                    Persons?.Add(person);
                }
            }

            foreach (var message in messages)
            {
                if (!(Messages?.Any(prototype => prototype.Id == message.Id) ?? false))
                {
                    Messages?.Add(message);
                }
            }

            if (countPerson != CountPerson || countMessage != CountMessage)
            {
                ChangeDataBase?.Invoke();
            }
        }
    }
}
