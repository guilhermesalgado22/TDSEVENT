using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PlanejaiFront.Models;
using PlanejaiFront.Models.APIConnection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlanejaiFront.Pages.Events
{
    public class Details : PageModel
    {
        [BindProperty]
        public EventModel? Event { get; set; } = new();

        public List<GuestModel> GuestList { get; set; } = new();

        public string OrganizerFullName;

        readonly HttpContext httpContext;

        public Details (IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext!;
            OrganizerFullName = $"{httpContext.Session.GetString("UserName")} {httpContext.Session.GetString("UserLastName")}";
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/EventsByUser/{httpContext.Session.GetInt32("UserID")}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            var eventsList = JsonConvert.DeserializeObject<List<EventModel>>(content);

            Event = eventsList!.Where(e => e.EventId == id).FirstOrDefault();

            httpClient = new HttpClient();
            url = $"{APIConnection.URL}/GuestsByEvent/{id}";
            requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            response = await httpClient.SendAsync(requestMessage);
            content = await response.Content.ReadAsStringAsync();

            var EventGuestsList = JsonConvert.DeserializeObject<List<EventsGuests>>(content);

            foreach (var eg in EventGuestsList!)
            {
                httpClient = new HttpClient();
                url = $"{APIConnection.URL}/Guests/{eg.GuestId}";
                requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

                response = await httpClient.SendAsync(requestMessage);
                content = await response.Content.ReadAsStringAsync();

                var guest = JsonConvert.DeserializeObject<GuestModel>(content);

                if (guest != null)
                    GuestList.Add(guest);
            }

            return Page();
        }
    }
}
