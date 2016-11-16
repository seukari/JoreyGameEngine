using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Jorey.Console.Game;
using Jorey.Console.Game.UI;
using Jorey.Console.Game.Events;

namespace Jorey.Console.Game
{
    public static class GameOverlord
    {
        public static EventList eventList;
        public static int originalDifficulty;
        public static int currentDifficulty;

        public static string menuSelectionStart = "[ ";
        public static string menuSelectionEnd = " ]";

        public static void Start()
        {

            Thread threadMain = new Thread(new ThreadStart(Main));
            //Thread threadInput = new Thread(new ThreadStart(UI.UI.InputLoop));

            threadMain.Start();
            //threadInput.Start();
        }

        public static void Main()
        {
            while (true)
            {

            }
        }
    }

    public static class Math
    {
        public static float Clamp(float variableNumber, float min, float max)
        {
            if (variableNumber < min) variableNumber = min;
            if (variableNumber > max) variableNumber = max;
            return variableNumber;
        }

        public static int Clamp(int variableNumber, int min, int max)
        {
            if (variableNumber < min) variableNumber = min;
            if (variableNumber > max) variableNumber = max;
            return variableNumber;
        }
    }
}

namespace Jorey.Console.Game.UI
{
    public static class UI
    {
        public static bool instantText;
        public static float textAppearSpeed = 0.2f;

        public static bool inputDisabled = false;

        public static ConsoleKey lastKeyPressed;        

        private static int selectedY = 0;
        private static int selectedX = 0;

        private static int maxY = 0;
        private static int maxX = 0;

        private static OptionScene lastScene;
        public static EventResponse lastResponse;

        public static void WriteScene(OptionScene scene)
        {
            if (scene.GetType() == typeof(Event))
                WriteEvent((Event)scene);
        }

        public static void WriteEvent(Event selectedEvent)
        {
            //ResetSelections();
            System.Console.Clear();

            System.Console.Title = selectedEvent.name;
            System.Console.WriteLine(selectedEvent.description);

            int index = 0;

            foreach(EventOption eo in selectedEvent.options)
            {
                string message = (selectedY == index) ? GameOverlord.menuSelectionStart + eo.description + GameOverlord.menuSelectionEnd : eo.description;
                System.Console.WriteLine(message);
                index++;
            }

            maxY = selectedEvent.options.Count-1;

            System.Console.WriteLine();
            selectedEvent.hasOccurred = true;

            lastScene = selectedEvent;
        }

        public static void WriteClear(object text)
        {
            string txt = (string)text;
            System.Console.Clear();
            System.Console.Write(txt);
        }

        public static void WriteClearLine(object text)
        {
            string txt = (string)text;
            System.Console.Clear();
            System.Console.Write(txt + Environment.NewLine);
        }

        public static void ResetSelections()
        {
            selectedX = 0;
            selectedY = 0;
        }

        public static ConsoleKey GetInput()
        {
            if(!inputDisabled)
            {
                ConsoleKeyInfo key = System.Console.ReadKey(true);

                if (key.Key == ConsoleKey.UpArrow) selectedY -= 1;
                if (key.Key == ConsoleKey.DownArrow) selectedY += 1;
                if (key.Key == ConsoleKey.LeftArrow) selectedX -= 1;
                if (key.Key == ConsoleKey.RightArrow) selectedX -= 1;

                selectedY = Math.Clamp(selectedY,0,maxY);
                selectedX = Math.Clamp(selectedX, 0, maxX);

                WriteScene(lastScene);        

                lastKeyPressed = key.Key;
                return lastKeyPressed;
            }
            return ConsoleKey.NoName;
        }

        public static void ChosenResponse(EventResponse response)
        {
            lastResponse = response;
            SendMessage(response.function);
        }
    }
}

namespace Jorey.Console.Game.Events
{

    public class OptionScene
    {
        public int id;
        public string name;
        public string description;
        public bool hasOccurred;

        public List<EventOption> options { get; protected set; } = new List<EventOption>();
        protected int nextIndex = 0;

        public OptionScene()
        {
            Constructor();
        }

        public OptionScene(string eventName, string eventDescription)
        {
            name = eventName;
            description = eventDescription;
        }

        public OptionScene(string eventName, string eventDescription, List<EventOption> eventOptions)
        {
            name = eventName;
            description = eventDescription;
            foreach (EventOption eo in eventOptions)
            {
                eo.id = nextIndex++;
            }
            options = eventOptions;
        }

        protected void Constructor()
        {

        }

        protected void Constructor(string eventName, string eventDescription)
        {
            name = eventName;
            description = eventDescription;
        }

        protected void Constructor(string eventName, string eventDescription, List<EventOption> eventOptions)
        {
            name = eventName;
            description = eventDescription;
            foreach (EventOption eo in eventOptions)
            {
                eo.id = nextIndex++;
            }
            options = eventOptions;
        }
    }

    public class EventList : List<Event>
    {
        public int nextIndex = 0;

        public new void Add(Event item)
        {
            item.id = nextIndex++;
            base.Add(item);
        }

        public void AddRange(List<Event> items)
        {
            foreach (Event e in items)
            {
                Add(e);
            }
        }

        public Event Get(int index)
        {
            foreach (Event e in this)
            {
                if (e.id == index) return e;
            }
            return new Event();
        }
    }


    public class Event : OptionScene
    {
        public Event()
        {
            base.Constructor();       
        }

        public Event(string eventName, string eventDescription)
        {
            base.Constructor(eventName, eventDescription);
        }

        public Event(string eventName, string eventDescription, List<EventOption> eventOptions)
        {
            base.Constructor(eventName, eventDescription, eventOptions);
        }

        public void AddEventOption(EventOption newOption)
        {
            newOption.id = nextIndex++;
            options.Add(newOption);
        }

        public void AddEventOptions(List<EventOption> newOptions)
        {
            foreach (EventOption eo in newOptions)
            {
                AddEventOption(eo);
            }
        }
    }

    public class EventOption
    {
        public int id;
        public string description;

        public EventResponse response;

        public EventOption(string optionDescription)
        {
            description = optionDescription;
        }

        public EventOption(string optionDescription, EventResponse eventResponse)
        {
            description = optionDescription;
            response = eventResponse;
        }
    }

    public class EventResponse
    {
        public event EventResponseSelected Selected;
        public string function;

        public EventResponse(string functionName)
        {
            function = functionName;   
        }

        public void Select()
        {
            UI.UI.ResetSelections();
            UI.UI.ChosenResponse(this);
            Selected(this, new GameEventArgs(false, ""));                     
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
