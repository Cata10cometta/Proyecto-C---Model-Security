using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class RolDTO
    {
        public int Id { get; set; }
        public string TypeRol { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public  Boolean Active { get; set; }

    }
}
