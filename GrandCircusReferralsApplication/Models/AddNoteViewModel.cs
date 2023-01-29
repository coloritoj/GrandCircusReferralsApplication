namespace GrandCircusReferralsApplication.Models
{
    public class AddNoteViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public List<BaseNote> Notes { get; set; }
    }
}
