using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Rest_API_CV.Models
{
    public class Employment
    {
        public int Id { get; set; }
        public string JobbTitle { get; set; }
        public string Company { get; set; }
        public DateTime StartOfEmployment { get; set; }
        public DateTime EndOfEmployment { get; set; }
    }
}

