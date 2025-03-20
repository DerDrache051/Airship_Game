using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airship_Game.Game.Core.Events
{
    public interface IEventAction<T>
    {
        public void EventAction(T t) { }
    }
}