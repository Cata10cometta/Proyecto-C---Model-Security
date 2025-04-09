using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Enum;


namespace Entity.Model
{
    public class RolPermission
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public Rol Rol { get; set; }
        public int PermissionId { get; set; }
        public Permission Permission  { get; set; }
        
    }
}