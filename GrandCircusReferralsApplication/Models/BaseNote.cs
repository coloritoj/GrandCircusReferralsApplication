namespace GrandCircusReferralsApplication.Models
{
    public class BaseNote
    {
        public int NoteID { get; set; }

        public int CandidateID { get; set; }

        public DateTime Date { get; set; }

        public string Note { get; set; }
    }
}
