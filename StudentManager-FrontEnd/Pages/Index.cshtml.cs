using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace StudentManager_FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _httpClient;

        public IndexModel(ILogger<IndexModel> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnGet()
        {
            // Make a GET request to the API endpoint
            var response = await _httpClient.GetAsync("https://localhost:7089/Student");

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Read the response content
                var responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into a list of items
                var str = responseBody;

                // You can now use 'items' in your Razor Page to display the data
                ViewData["str"] = str;
            }
            else
            {
                // Handle the error
                // For example, you can set an error message to be displayed in the Razor Page
                ViewData["ErrorMessage"] = "Failed to retrieve data from the API.";
            }

            return Page();
        }

    }
}
