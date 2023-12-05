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

namespace PlanejaiFront.Pages.Activities
{
    public class Edit : PageModel
    {
        [BindProperty]
        public ActivityModel ActivityToEdit { get; set; } = new();
        public bool DatesAreValid { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/Activities/{id}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var existingActivity = JsonConvert.DeserializeObject<ActivityModel>(content);

                ActivityToEdit = existingActivity!;

                return Page();
            }

            return RedirectToPage("/Schedules/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/Schedules/{ActivityToEdit.ScheduleId}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            var schedule = JsonConvert.DeserializeObject<ScheduleModel>(content);

            httpClient = new HttpClient();
            url = $"{APIConnection.URL}/Events/{schedule!.EventId}";
            requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            response = await httpClient.SendAsync(requestMessage);
            content = await response.Content.ReadAsStringAsync();
            var eventModel = JsonConvert.DeserializeObject<EventModel>(content);

            schedule.Event = eventModel!;

            DatesAreValid = ActivityToEdit.DatesAreValid(schedule!);

            if (!ModelState.IsValid || !DatesAreValid)
            {
                if (!DatesAreValid)
                {
                    ModelState.AddModelError("DatesAreValid", "Os horários das atividades devem estar entre o início e final do evento.");
                }

                return Page();
            }

            httpClient = new HttpClient();
            url = $"{APIConnection.URL}/EditActivity/{ActivityToEdit.ActivityId}";
            requestMessage = new HttpRequestMessage(HttpMethod.Put, url);
            var jsonActivity = JsonConvert.SerializeObject(ActivityToEdit);
            requestMessage.Content = new StringContent(jsonActivity, Encoding.UTF8, "application/json");
            response = await httpClient.SendAsync(requestMessage);

            content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Schedules/Details", new { id = ActivityToEdit.ScheduleId });
            }
            else
            {
                ModelState.AddModelError(string.Empty, content);

                return Page();
            }
        }
    }
}
