using System;
using System.IO;
using System.Threading;
using BusinessLogic.Model;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Soap;

namespace BusinessLogic.Control
{
    internal static class Synchronization
    {
        internal static event Func<List<Message>, bool>? UpdateDataBase;

        private static SoapFormatter Formatter;

        private static Thread UpdateThread;

        private static EventWaitHandle? Event;
        private static Mutex Mutex;

        static Synchronization()
        {
            UpdateThread = new Thread(Task);
            UpdateThread.IsBackground = true;
            UpdateThread.Start();

            Formatter = new SoapFormatter();

            Mutex = new Mutex(false, "CustomSerialization");

            if (!EventWaitHandle.TryOpenExisting("EventSimpleChat", out Event))
            {
                Event = new EventWaitHandle(false, EventResetMode.ManualReset, "EventSimpleChat");
            }
        }

        private static void Task()
        {
            while (true)
            {
                Event?.WaitOne();

                if (!Deserialize())
                {
                    Event?.Reset();
                }
            }
        }

        internal static void Serialize()
        {
            Deserialize();

            Mutex.WaitOne();

            using (var file = new FileStream("CustomSerialization.soap", FileMode.Create))
            {
                Formatter.Serialize(file, DataBase.Messages.ToArray());
            }

            Mutex.ReleaseMutex();

            Event?.Set();
        }

        internal static bool Deserialize()
        {
            List<Message> newMessages;

            if (File.Exists("CustomSerialization.soap"))
            {
                Mutex.WaitOne();

                using (var file = new FileStream("CustomSerialization.soap", FileMode.Open))
                {
                    var newArray = Formatter.Deserialize(file) as Message[];

                    if (newArray != null)
                    {
                        newMessages = new List<Message>(newArray);
                    }
                    else
                    {
                        newMessages = new List<Message>();
                    }
                }

                Mutex.ReleaseMutex();

                return UpdateDataBase?.Invoke(newMessages) ?? false;
            }

            return false;
        }
    }
}
