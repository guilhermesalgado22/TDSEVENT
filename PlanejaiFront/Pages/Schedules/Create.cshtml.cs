using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PlanejaiFront.Models;
using PlanejaiFront.Models.APIConnection;
using PlanejaiFront.Pages.Guests;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlanejaiFront.Pages.Schedules
{
    [BindProperties]
    public class Create : PageModel
    {
        public ScheduleModel NewSchedule { get; set; } = new();
        public List<EventModel> EventsList { get; set; } = new();

        public int eventId { get; set; }

        readonly HttpContext httpContext;
        public Create(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/EventsByUser/{httpContext.Session.GetInt32("UserID")}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            var eventsList = JsonConvert.DeserializeObject<List<EventModel>>(content);

            EventsList = eventsList!;

            if (EventsList.Count == 0)
            {
                return RedirectToPage("/Events/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await OnGetAsync();

            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/Schedules/";

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            var jsonSchedule = JsonConvert.SerializeObject(NewSchedule);
            requestMessage.Content = new StringContent(jsonSchedule, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Schedules/Index");
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
