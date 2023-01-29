using GrandCircusReferralsApplication.HelperClasses;
using GrandCircusReferralsApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        
        public async Task<IActionResult> AddNote(BaseUser baseUser)
        {
            AddNoteViewModel addNoteViewModel = new AddNoteViewModel() { Name = baseUser.Name, ID = baseUser.ID };         

            HttpClient client = new HttpClient() { BaseAddress = new Uri("https://localhost:7021") };
            var response = await client.GetAsync($"/api/GetNotesByCandidateID?candidateID={baseUser.ID}");
            addNoteViewModel.Notes = await response.Content.ReadFromJsonAsync<List<BaseNote>>();

            addNoteViewModel.Notes = addNoteViewModel.Notes.OrderByDescending(x => x.NoteID).ToList();

            return View(addNoteViewModel);
        }

        public async Task<IActionResult> DeleteNote(int candidateID, string name, int noteID)
        {
            HttpClient client = new HttpClient() { BaseAddress = new Uri("https://localhost:7021") };
            var response = await client.DeleteAsync($"/api/DeleteNoteByNoteID?noteID={noteID}");

            BaseUser baseUser = new BaseUser() { ID = candidateID, Name = name };

            return RedirectToAction("AddNote", "Users", baseUser);
        }
    }
}
