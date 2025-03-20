using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airship_Game.Game.Core.Events
{
    public static class EventManager
    {
        public static Dictionary<Type, object> eventActions = new();
        public static Dictionary<Type, object> eventChecks = new();
        public static LinkedList<IEventAction<Event>> universalActions = new();
        public static void addEventAction<T>  (IEventAction<T> action)where T:Event
        {
            if (!eventActions.ContainsKey(typeof(T)))
            {
                eventActions.Add(typeof(T), new LinkedList<IEventAction<T>>());
            }
            if (eventActions[typeof(T)] is LinkedList<IEventAction<T>> actions)
            {
                actions.AddLast(action);
            }
        }
        public static void addEventCheck<T>(IEventCheck<T> check)where T:CancelableEvent
        {
            if (!eventChecks.ContainsKey(typeof(T)))
            {
                eventChecks.Add(typeof(T), new LinkedList<IEventCheck<T>>());
            }
            if (eventChecks[typeof(T)] is LinkedList<IEventCheck<T>> checks)
            {
                checks.AddLast(check);
            }
        }
        public static void removeEventAction<T>(IEventAction<T> action)where T:Event
        {
            if (eventActions[typeof(T)] is LinkedList<IEventAction<T>> actions)
            {
                actions.Remove(action);
            }
        }
        public static void removeEventCheck<T>(IEventCheck<T> check)where T:CancelableEvent
        {
            if (eventChecks[typeof(T)] is LinkedList<IEventCheck<T>> checks)
            {
                checks.Remove(check);
            }
        }
        public static bool triggerEvent<T>(T t)where T:Event
        {

            if (eventChecks[typeof(T)] is LinkedList<IEventCheck<T>> checks)
            {
                foreach (IEventCheck<T> check in checks)
                {
                    if (!check.CheckEvent(t)) return false;
                }
            }
            if (eventActions[typeof(T)] is LinkedList<IEventAction<T>> actions)
            {
                foreach (IEventAction<T> action in actions)
                {
                    action.EventAction(t);
                }
            }
            foreach (IEventAction<Event> action in universalActions)
            {
                action.EventAction(t);
            }
            return true;
        }
        public static void addUniversalAction(IEventAction<Event> action)
        {
            universalActions.AddLast(action);   
        }
        public static void removeUniversalAction(IEventAction<Event> action)
        {
            universalActions.Remove(action);
        }

    }

}