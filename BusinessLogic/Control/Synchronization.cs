using System;
using System.IO;
using System.Threading;
using BusinessLogic.Model;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

namespace BusinessLogic.Control
{
    internal static class Synchronization
    {
        internal static event Func<List<Message>, bool>? UpdateDataBase;

        private static DataContractJsonSerializer Serializer;

        private static EventWaitHandle? Event;
        private static Mutex Mutex;

        private static Thread UpdateThread;

        static Synchronization()
        {
            Serializer = new DataContractJsonSerializer(typeof(List<Message>));

            UpdateThread = new Thread(Task);
            UpdateThread.IsBackground = true;
            UpdateThread.Start();

            Mutex = new Mutex(false, "NativeSerialization");

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

            using (var file = new FileStream("NativeSerialization.json", FileMode.Create))
            {
                Serializer.WriteObject(file, DataBase.Messages);
            }

            Mutex.ReleaseMutex();

            Event?.Set();
        }

        internal static bool Deserialize()
        {
            List<Message> newMessages;

            if (File.Exists("NativeSerialization.json"))
            {
                Mutex.WaitOne();

                using (var file = new FileStream("NativeSerialization.json", FileMode.Open))
                {
                    newMessages = Serializer.ReadObject(file) as List<Message> ?? new List<Message>();
                }

                Mutex.ReleaseMutex();

                return UpdateDataBase?.Invoke(newMessages) ?? false;
            }

            return false;
        }
    }
}
