using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PlanejaiFront.Models;
using PlanejaiFront.Models.APIConnection;

namespace PlanejaiFront.Pages.Schedules
{
    public class Index : PageModel
    {
        [BindProperty]
        public List<ScheduleModel>? SchedulesList { get; set; }

        readonly HttpContext httpContext;
        public Index(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (httpContext.Session.GetInt32("UserID") == null)
            {
                return RedirectToPage("/User/Login");
            }

            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/SchedulesByUser/{httpContext.Session.GetInt32("UserID")}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            var schedulesList = JsonConvert.DeserializeObject<List<ScheduleModel>>(content);
            SchedulesList = schedulesList;

            return Page();
        }
    }
}
