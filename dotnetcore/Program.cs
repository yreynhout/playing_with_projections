using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace projections
{
    public class Program
    {
        public static void Main(string[] args)
        {
           var stream_id = 0;
           if(args.Count() > 0) stream_id = int.Parse(args[0]);
           //var raw_data = new RestEventReader().Read($"http://playing-with-projections.herokuapp.com/stream/{stream_id}");
           //var raw_data = new RestEventReader().Read($"https://raw.githubusercontent.com/tcoopman/playing_with_projections_server/master/data/{stream_id}.json");
           var raw_data = new FileEventReader().Read($"../data/{stream_id}.json");
           var events = new JsonEventParser().Parse(raw_data);
           var projector = new Projector(events);

           Console.WriteLine("Number of events: {0}", projector.NumberOfEvents);
           Console.WriteLine("Number of players that registered: {0}", projector.NumberOfRegisteredPlayers);
        }
    }

    public class Projector
    {
      readonly IEnumerable<Event> events;
      public Projector(IEnumerable<Event> events)
      {
        this.events = events;
      }
      public int NumberOfEvents{get{ return events.Count();}}
      public int NumberOfRegisteredPlayers 
      {
        get 
        {
          return events.Where(@event => @event.type == "PlayerHasRegistered").Count();
        }
      }
      //public string[] DistinctEvents{get{return events.GroupBy()}}
    }
}
