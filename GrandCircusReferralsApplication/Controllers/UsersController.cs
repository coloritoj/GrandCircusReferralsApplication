using GrandCircusReferralsApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace GrandCircusReferralsApplication.Controllers
{
    public class UsersController : Controller
    {
        public async Task<IActionResult> Index()
        {
            HttpClient client = new HttpClient() { BaseAddress = new Uri("https://localhost:7021") };
            var response = await client.GetAsync("/api/Users");
            List<BaseUser> users = await response.Content.ReadFromJsonAsync<List<BaseUser>>();

            foreach (var user in users)
            {
                user.Notes = user.Notes.OrderByDescending(x => x.NoteID).ToList();
                user.LatestNote = user.Notes.FirstOrDefault();
            }

            return View(users);
        }
    }
}
