using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PlanejaiFront.Models;
using PlanejaiFront.Models.APIConnection;

namespace PlanejaiFront.Pages.Guests
{
    [BindProperties]
    public class Remove : PageModel
    {
        public EventModel Event { get; set; } = new();
        public GuestModel Guest { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int eventId, int guestId)
        {
            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/Events/{eventId}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            var eventModel = JsonConvert.DeserializeObject<EventModel>(content);

            Event = eventModel!;

            httpClient = new HttpClient();
            url = $"{APIConnection.URL}/Guests/{guestId}";
            requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            response = await httpClient.SendAsync(requestMessage);
            content = await response.Content.ReadAsStringAsync();
            var guestModel = JsonConvert.DeserializeObject<GuestModel>(content);

            Guest = guestModel!;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int eventId, int guestId)
        {
            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/Guests/{eventId}/{guestId}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url);

            var response = await httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Events/Details", new { id = eventId });
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, content);

                return Page();
            }
        }
    }
}
