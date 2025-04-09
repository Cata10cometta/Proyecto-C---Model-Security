using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class ExperienceLineThematicDTO
    {
        public int Id { get; set; }

        public int LineThematicId { get; set; }
        public int ExperienceId { get; set; }
    }
}
