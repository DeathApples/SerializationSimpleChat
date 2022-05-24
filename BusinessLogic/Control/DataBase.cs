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

            Synchronization.UpdateDataBase += UpdateDataBaseHandle;
        }

        public static void EditCurrentPerson(string name)
        {
            Current.Name = name;
            Synchronization.Serialize();
        }

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

        internal static bool UpdateDataBaseHandle(List<Message> messages)
        {
            int countPerson = CountPerson, countMessage = CountMessage;

            if (messages == null)
                throw new NullReferenceException("messages");

            foreach (var message in messages)
            {
                if (!(Messages?.Any(prototype => prototype.Id == message.Id) ?? false))
                {
                    Messages?.Add(message);
                }
            }

            foreach (var message in Messages)
            {
                if (!(Persons?.Any(prototype => prototype.Id == message.Person.Id) ?? false))
                {
                    Persons?.Add(message.Person);
                }
                else
                {
                    Persons.Find(prototype => prototype.Id == message.Person.Id).Name = message.Person.Name;
                }
            }

            Messages.Sort();

            if (countPerson != CountPerson || countMessage != CountMessage)
            {
                ChangeDataBase?.Invoke();
                return true;
            }

            return false;
        }
    }
}
