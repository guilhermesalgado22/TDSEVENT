using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PlanejaiFront.Models;
using PlanejaiFront.Models.APIConnection;

namespace PlanejaiFront.Pages.Activities
{
    [BindProperties]
    public class Remove : PageModel
    {
        public ActivityModel Activity { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/Activities/{id}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            var activityModel = JsonConvert.DeserializeObject<ActivityModel>(content);

            Activity = activityModel!;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/Activities/{id}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url);

            var response = await httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Schedules/Details", new { id = Activity.ScheduleId });
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
