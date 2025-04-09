using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class ExperienceLineThematic
    {
        public int Id { get; set; }
        public int  LineThematicId { get; set; }
        public LineThematic LineThematic { get; set;}
        public  int ExperienceId { get; set; }
        public Experience Experience { get; set; }
        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}