using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PlanejaiFront.Models;
using PlanejaiFront.Models.APIConnection;
using System.Net.Http;
using System.Text;

namespace PlanejaiFront.Pages.Activities
{
    public class Add : PageModel
    {
        [BindProperty]
        public ActivityModel NewActivity { get; set; } = new();
        public bool DatesAreValid { get; set; }
        public ScheduleModel Schedule { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            Schedule.ScheduleId = id;
            Console.WriteLine(Schedule.ScheduleId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/Schedules/{id}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            var schedule = JsonConvert.DeserializeObject<ScheduleModel>(content);
            Schedule = schedule!;

            DatesAreValid = NewActivity.DatesAreValid(Schedule);

            if (!ModelState.IsValid || !DatesAreValid)
            {
                if (!DatesAreValid)
                {
                    ModelState.AddModelError("DatesAreValid", "Os horários das atividades devem estar entre o início e final do evento.");
                }

                return Page();
            }

            NewActivity.ScheduleId = Schedule.ScheduleId;

            httpClient = new HttpClient();
            url = $"{APIConnection.URL}/Activities/";

            requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            var jsonActivity = JsonConvert.SerializeObject(NewActivity);
            requestMessage.Content = new StringContent(jsonActivity, Encoding.UTF8, "application/json");

            response = await httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Schedules/Details", new { id = Schedule.ScheduleId });
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();

                ModelState.AddModelError(string.Empty, errorResponse);

                return Page();
            }
        }
    }
}
