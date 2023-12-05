using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PlanejaiFront.Pages.Calendar
{
    public class Index : PageModel
    {
        [BindProperty]
        public int Month { get; set; }
        [BindProperty]
        public int Year { get; set; }

        public void OnGet(int month, int year)
        {
            if (month != 0 && year != 0)
            {
                Month = month;
                Year = year;
            }
        }

        public IActionResult OnPostAsync(string monthOffset)
        {
            if (monthOffset == "<")
            {
                if (Month! == 1)
                {
                    return Redirect($"/Calendar/12/{Year - 1}");
                }
                return Redirect($"/Calendar/{Month - 1}/{Year}");
            }
            else
            {
                if (Month == 12)
                {
                    return Redirect($"/Calendar/1/{Year + 1}");
                }
                return Redirect($"/Calendar/{Month + 1}/{Year}");
            }
        }
    }
}
