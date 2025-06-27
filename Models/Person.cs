namespace Rest_API_CV.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Adress { get; set; }
        public int PhoneNumber { get; set; }

        public List<Education> Educations { get; set; }

        public List<Employment> Employments { get; set; }
    }
}
