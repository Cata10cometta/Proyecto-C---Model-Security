using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class ExperiencePopulationGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ExperienceId { get; set; }
        public int PopulationGradeId { get; set; }


    }
}
