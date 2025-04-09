using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class ExperienceDTO
    {
        public int Id { get; set; }
        public string NameExperiences { get; set; }
        public string Summary { get; set; }
        public string Methodologies { get; set; }
        public string Transfer { get; set; }
        public DateTime DateRegistration { get; set; }
        public string code { get; set; }    
        public int UserId { get; set; }
        public int InstitutionId { get; set; }



    }
}
