namespace GrandCircusReferralsApplication.Models
{
    public class BaseUser
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Bootcamp { get; set; }

        public int GraduationFlag { get; set; }

        public int GraduationYear { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string EmploymentStatus { get; set; }

        public string Employer { get; set; }

        public int InterestFlag { get; set; }

        public string ApplicationStatus { get; set; }
    }
}
