namespace Rest_API_CV.DTO
{

    public class PersonDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; } 

        public List<EducationDTO> Educations { get; set; }
        public List<EmploymentDTO> Employments { get; set; }
    }

}
