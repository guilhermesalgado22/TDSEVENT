using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PlanejaiFront.Models;
using PlanejaiFront.Models.APIConnection;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace PlanejaiFront.Pages.User
{
    public class Login : PageModel
    {
        public UserModel? ExistingUser { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Informe um e-mail v�lido.")]
        public string? Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Informe uma senha.")]
        public string? Password { get; set; }

        readonly HttpContext httpContext;
        public Login (IHttpContextAccessor httpContextAccessor) 
        {
            httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var httpClient = new HttpClient();
            var url = $"{APIConnection.URL}/Users/{Email}/{Password}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var existingUser = JsonConvert.DeserializeObject<UserModel>(content);

                ExistingUser = existingUser;

                httpContext.Session.SetInt32("UserID", ExistingUser!.UserId);
                httpContext.Session.SetString("UserName", ExistingUser!.Name!);
                httpContext.Session.SetString("UserLastName", ExistingUser!.LastName!);

                return RedirectToPage("/Events/Index");
            }
            else
            {
                ModelState.AddModelError("ExistingUser", content);

                return Page();
            }
        }
    }
}
