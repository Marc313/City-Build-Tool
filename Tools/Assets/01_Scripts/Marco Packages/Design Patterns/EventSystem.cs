using System;
using System.Collections.Generic;

namespace MarcoHelpers
{
    public enum EventName
    {
        PRESETS_LOADED = 0,
        TAB_CHANGED = 1,
        ON_OBJNAME_ALREADY_EXISTS = 2,
        IMPORT_SUCCESS = 3,
        MENU_OPENED = 4,
        MENU_CLOSED = 5,
    }

    public delegate void EventCallback(object _value);

    public static class EventSystem
    {
        private static Dictionary<EventName, List<EventCallback>> eventRegister = new Dictionary<EventName, List<EventCallback>>();

        public static void Subscribe(EventName _evt, EventCallback _func)
        {
            if (!eventRegister.ContainsKey(_evt))
            {
                eventRegister[_evt] = new List<EventCallback>();
            }

            eventRegister[_evt].Add(_func);
        }

        public static void Unsubscribe(EventName _evt, EventCallback _func)
        {
            if (eventRegister.ContainsKey(_evt))
            {
                eventRegister[_evt].Remove(_func);
            }
        }

        public static void RaiseEvent(EventName _evt, object _value = null)
        {
            if (eventRegister.ContainsKey(_evt))
            {
                foreach (EventCallback e in eventRegister[_evt])
                {
                    e.Invoke(_value);
                }
            }
        }
    }

    //public delegate void EventCallback(object _value);

    /*    public static class EventSystem
        {
            private static Dictionary<EventName, List<Action>> eventRegister = new Dictionary<EventName, List<Action>>();

            public static void Subscribe(EventName _evt, Action _func)
            {
                if (!eventRegister.ContainsKey(_evt))
                {
                    eventRegister[_evt] = new List<Action>();
                }

                eventRegister[_evt].Add(_func);
            }

            public static void Unsubscribe(EventName _evt, Action _func)
            {
                if (eventRegister.ContainsKey(_evt))
                {
                    eventRegister[_evt].Remove(_func);
                }
            }

            public static void RaiseEvent(EventName _evt, object _value = null)
            {
                if (eventRegister.ContainsKey(_evt))
                {
                    foreach (Action e in eventRegister[_evt])
                    {
                        e.Invoke();
                    }
                }
            }
        }*/
} 

