namespace GrandCircusReferralsApplication.Models
{
    public class AddNewUserModel
    {
        public string Name { get; set; }

        public int Bootcamp { get; set; }

        public int GraduationFlag { get; set; }

        public int GraduationYear { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int EmploymentStatus { get; set; }

        public string Employer { get; set; }

        public int InterestFlag { get; set; }

        public int ApplicationStatus { get; set; }

        public string Note { get; set; }
    }
}
