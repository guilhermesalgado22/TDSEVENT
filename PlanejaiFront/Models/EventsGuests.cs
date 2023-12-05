using Newtonsoft.Json;

namespace PlanejaiFront.Models
{
    public class EventsGuests
    {
        public int EventId { get; set; }
        [JsonIgnore]
        public EventModel? Event { get; set; }
        public int GuestId { get; set; }
        [JsonIgnore]
        public GuestModel? Guest { get; set; }
    }
}
