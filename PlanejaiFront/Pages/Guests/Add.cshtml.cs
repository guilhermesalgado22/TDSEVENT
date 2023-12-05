using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PlanejaiFront.Models;
using PlanejaiFront.Models.APIConnection;
using System.Text;

namespace PlanejaiFront.Pages.Guests
{
    public class Add : PageModel
    {
        [BindProperty]
        public GuestModel NewGuest { get; set; } = new();
        [BindProperty]
        public int eventId { get; set; }

        public async Task<ActionResult> OnPostAsync(int id)
        {
            eventId = id;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/AddGuest/{eventId}";

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            var jsonGuest = JsonConvert.SerializeObject(NewGuest);
            requestMessage.Content = new StringContent(jsonGuest, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Events/Details", new { id = eventId });
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();

                ModelState.AddModelError("eventId", errorResponse);

                return Page();
            }
        }
    }
}
