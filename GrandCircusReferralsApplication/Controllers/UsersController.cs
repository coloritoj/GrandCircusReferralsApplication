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

            users = users.OrderByDescending(x => x.ApplicationStatus == "Hired")
                .ThenByDescending(x => x.ApplicationStatus == "Applied")
                .ThenByDescending(x => x.ApplicationStatus == "Waiting to Apply")
                .ThenByDescending(x => x.ApplicationStatus == "Not Applied")
                .ThenByDescending(x => x.ApplicationStatus == "Rejected")
                .ThenByDescending(x => x.InterestFlag)
                .ThenBy(x => x.Bootcamp)
                .ToList();

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

        public IActionResult AddNewUser()
        {
            return View();
        }

        public async Task<IActionResult> PostUser(string name, int bootcamp, int graduationFlag,
                                                  int graduationYear, string city, string state,
                                                  int employmentStatus, string employer, int interestFlag,
                                                  int applicationStatus, string note)
        {
            AddNewUserModel addNewUserModel = new AddNewUserModel()
            {
                Name = name,
                Bootcamp = bootcamp,
                GraduationFlag = graduationFlag,
                GraduationYear = graduationYear,
                City = city,
                State = state,
                EmploymentStatus = employmentStatus,
                Employer = employer,
                InterestFlag = interestFlag,
                ApplicationStatus = applicationStatus,
                Note = note
            };

            HttpClient client = new HttpClient() { BaseAddress = new Uri("https://localhost:7021") };
            var endpoint = new Uri("https://localhost:7021/api/AddNewUser");
            var newJson = JsonConvert.SerializeObject(addNewUserModel);
            var payload = new StringContent(newJson, Encoding.UTF8, "application/json");
            var result = await client.PostAsync(endpoint, payload);

            return RedirectToAction("Index", "Users");
        }

        public async Task<IActionResult> UpdateApplicationStatus(BaseUser user)
        {
            return View(user);
        }

        public async Task<IActionResult> PostUpdatedApplicationStatus(int interestFlag, int applicationStatus, string note, int candidateID)
        {
            UpdateApplicationStatusModel updateApplicationStatusModel = new UpdateApplicationStatusModel()
            {
                InterestFlag = interestFlag,
                ApplicationStatus = applicationStatus,
                Note = note,
                CandidateID = candidateID
            };

            HttpClient client = new HttpClient() { BaseAddress = new Uri("https://localhost:7021") };
            var endpoint = new Uri("https://localhost:7021/api/UpdateApplicationStatusByUserID");
            var newJson = JsonConvert.SerializeObject(updateApplicationStatusModel);
            var payload = new StringContent(newJson, Encoding.UTF8, "application/json");
            var result = await client.PostAsync(endpoint, payload);

            return RedirectToAction("Index", "Users");
        }
    }
}
