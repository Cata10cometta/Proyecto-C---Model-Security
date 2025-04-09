using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class PermissionDTO
    {
        public int Id { get; set; }
        public string PermissionType { get; set; }
        public int RolId { get; set; }

    }
}
