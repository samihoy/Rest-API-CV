namespace Rest_API_CV.Models
{
    public class Education
    {
        public int Id { get; set; }
        public string SchoolName { get; set; }
        public string Diploma { get; set; }
        public DateTime GraduationDate { get; set; }
        public DateTime EnrolmentDate { get; set; }
    }
}
