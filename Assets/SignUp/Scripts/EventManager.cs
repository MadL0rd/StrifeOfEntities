using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace GameSparksTutorials
{
    public class EventManager : MonoBehaviour
    {
        private Dictionary<string, UnityEventBase> eventDictionary;

        private static EventManager eventManager;

        public static EventManager Instance
        {
            get
            {
                if (!eventManager)
                {
                    eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                    if (!eventManager)
                    {
                        Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                    }
                    else
                    {
                        eventManager.Init();
                    }
                }

                return eventManager;
            }
        }

        void Init()
        {
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<string, UnityEventBase>();
            }
        }

        public static void StartListening(string eventName, UnityAction listener)
        {
            UnityEventBase thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                ((UnityEvent)thisEvent).AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                ((UnityEvent)thisEvent).AddListener(listener);
                Instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StartListening<T>(string eventName, UnityAction<T> listener)
        {
            string log = "Start listenning";

            UnityEventBase thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                log += " again";
                ((UnityEvent<T>)thisEvent).AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEventWrapper<T>();
                ((UnityEvent<T>)thisEvent).AddListener(listener);
                Instance.eventDictionary.Add(eventName, thisEvent);
            }

            log += " to " + eventName + ".";
            Debug.Log(log);
        }

        public static void StartListening<T, U>(string eventName, UnityAction<T, U> listener)
        {
            UnityEventBase thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                ((UnityEvent<T, U>)thisEvent).AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEventWrapper<T, U>();
                ((UnityEvent<T, U>)thisEvent).AddListener(listener);
                Instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, UnityAction listener)
        {
            if (eventManager == null) return;
            UnityEventBase thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                ((UnityEvent)thisEvent).RemoveListener(listener);
            }
        }

        public static void StopListening<T>(string eventName, UnityAction<T> listener)
        {
            if (eventManager == null) return;
            UnityEventBase thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                ((UnityEvent<T>)thisEvent).RemoveListener(listener);
            }
        }

        public static void StopListening<T, U>(string eventName, UnityAction<T, U> listener)
        {
            if (eventManager == null) return;
            UnityEventBase thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                ((UnityEvent<T, U>)thisEvent).RemoveListener(listener);
            }
        }

        public static void TriggerEvent(string eventName)
        {
            UnityEventBase thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                ((UnityEvent)thisEvent).Invoke();
            }
        }

        public static void TriggerEvent<T>(string eventName, T value)
        {
            UnityEventBase thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                ((UnityEvent<T>)thisEvent).Invoke(value);
            }
        }

        public static void TriggerEvent<T, U>(string eventName, T value1, U value2)
        {
            UnityEventBase thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                ((UnityEvent<T, U>)thisEvent).Invoke(value1, value2);
            }
        }

        private class UnityEventWrapper<T> : UnityEvent<T> { }

        private class UnityEventWrapper<T, U> : UnityEvent<T, U> { }
    }
}