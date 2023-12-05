using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PlanejaiFront.Models;
using PlanejaiFront.Models.APIConnection;
using System.Net.Http;
using System;
using System.Reflection.Metadata;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace PlanejaiFront.Pages.Events
{
    public class Edit : PageModel
    {
        [BindProperty]
        public EventModel EventToEdit { get; set; } = new();
        public bool DatesAreValid { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/Events/{id}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var existingActivity = JsonConvert.DeserializeObject<EventModel>(content);

                EventToEdit = existingActivity!;

                return Page();
            }

            return RedirectToPage("/Events/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            DatesAreValid = EventToEdit.DatesAreValid();

            if (!ModelState.IsValid || !DatesAreValid)
            {
                if (!DatesAreValid)
                {
                    ModelState.AddModelError("DatesAreValid", "Os horários das atividades devem estar entre o início e final do evento.");
                }

                return Page();
            }

            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/EditEvent/{EventToEdit.EventId}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, url);
            var jsonEvent = JsonConvert.SerializeObject(EventToEdit);
            requestMessage.Content = new StringContent(jsonEvent, Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(requestMessage);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Events/Details", new { id = EventToEdit.EventId });
            }
            else
            {
                ModelState.AddModelError(string.Empty, content);

                return Page();
            }
        }
    }
}
