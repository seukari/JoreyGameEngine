using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jorey.Console.Game;
using Jorey.Console.Game.Events;


namespace Jorey.Console.Game.Events
{

    public class EventList : List<Event>
    {
        
    }


    public class Event
    {
        public int id;
        public string name;
        public string description;
        public bool hasOccurred;

        public List<EventOption> eventOptions;

        public Event(string eventName, string eventDescription)
        {

        }
    }

    public class EventOption
    {
        public int id;
        public string eventOptionDescription;

        public EventResponse eventResponse;

        public EventOption()
        {

        }
    }

    public class EventResponse
    {
        public event EventResponseSelected Selected;

        public EventResponse()
        {

        }

        public void Select()
        {
            Selected(this, new GameEventArgs(false,""));
        }
    }

    public delegate void EventResponseSelected(object sender, GameEventArgs e);

    public class GameEventArgs : EventArgs
    {
        public bool errored;
        public string errorMessage;

        public GameEventArgs(bool isError, string message)
        {

        }
    }
        
}

namespace Jorey.Console.Game.UI
{
    public static class UI
    {
        public static bool instantText;
        public static float textAppearSpeed = 0.2f;

        private static int selectedOption = 0;

        public static void WriteEvent(Event selectedEvent)
        {
            System.Console.Title = selectedEvent.name;
            System.Console.WriteLine(selectedEvent.description);

            foreach(EventOption eo in selectedEvent.eventOptions)
            {
                
            }

        }
    }
}

namespace Jorey.Console.Game
{
    public static class GameOverlord
    {
        public static EventList eventList;
        public static int originalDifficulty;
        public static int currentDifficulty;
    }
}