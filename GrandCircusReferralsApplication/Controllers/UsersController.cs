using GrandCircusReferralsApplication.HelperClasses;
using GrandCircusReferralsApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

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
        
        public async Task<IActionResult> GetNotes(BaseUser baseUser)
        {   
            HttpClient client = new HttpClient() { BaseAddress = new Uri("https://localhost:7021") };
            var response = await client.GetAsync($"/api/GetNotesByCandidateID?candidateID={baseUser.ID}");
            baseUser.Notes = await response.Content.ReadFromJsonAsync<List<BaseNote>>();

            baseUser.Notes = baseUser.Notes.OrderByDescending(x => x.NoteID).ToList();

            return View(baseUser);
        }

        public async Task<IActionResult> GetNotesAfterDelete(int candidateID)
        {
            HttpClient client = new HttpClient() { BaseAddress = new Uri("https://localhost:7021") };
            var response = await client.GetAsync("/api/Users");
            List<BaseUser> users = await response.Content.ReadFromJsonAsync<List<BaseUser>>();

            BaseUser baseUser = users.Where(x => x.ID == candidateID).FirstOrDefault();

            baseUser.Notes = baseUser.Notes.OrderByDescending(x => x.NoteID).ToList();

            return View(baseUser);
        }

        public async Task<IActionResult> DeleteNote(int candidateID, int noteID)
        {
            HttpClient client = new HttpClient() { BaseAddress = new Uri("https://localhost:7021") };
            var response = await client.DeleteAsync($"/api/DeleteNoteByNoteID?noteID={noteID}");

            return RedirectToAction($"GetNotesAfterDelete", "Users", new {candidateID});
        }

        public async Task<IActionResult> PostNote(int userID, string note)
        {
            AddNoteModel model = new AddNoteModel() { CandidateID = userID, Note = note };

            HttpClient client = new HttpClient() { BaseAddress = new Uri("https://localhost:7021") };

            var endpoint = new Uri("https://localhost:7021/api/AddNoteByCandidateID");
            var newJson = JsonConvert.SerializeObject(model);
            var payload = new StringContent(newJson, Encoding.UTF8, "application/json");
            var result = await client.PostAsync(endpoint, payload);

            var response = await client.GetAsync("/api/Users");
            List<BaseUser> users = await response.Content.ReadFromJsonAsync<List<BaseUser>>();
            BaseUser baseUser = users.Where(x => x.ID == userID).FirstOrDefault();
            baseUser.Notes = baseUser.Notes.OrderByDescending(x => x.NoteID).ToList();

            return RedirectToAction("GetNotes", "Users", baseUser);
        }
    }
}
