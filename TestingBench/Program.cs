using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jorey.Console.Game;
using Jorey.Console.Game.Events;
using Jorey.Console.Game.UI;


namespace TestingBench
{
    class Program
    {
        static void Main(string[] args)
        {
            Event evnt1 = new Event("Event1", "This is a test event. I am doing a thing and typing a lot of stuff." + Environment.NewLine + "This is a second line. Blarg?");
            evnt1.AddEventOption(new EventOption("Blarg.", new EventResponse()));
            evnt1.AddEventOption(new EventOption("Blarg blarg?", new EventResponse()));
            evnt1.AddEventOption(new EventOption("Gloop.", new EventResponse()));

            UI.WriteEvent(evnt1);
            while(true)
            {
                UI.GetInput();
            }
        }

        static void Flush()
        {
            System.Console.Clear();
        }
    }
}
