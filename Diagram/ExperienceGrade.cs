//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Diagrama_Experiencia
{
    using System;
    using System.Collections.Generic;
    
    public partial class ExperienceGrade
    {
        public int Id { get; set; }
        public int GradeId { get; set; }
    
        public virtual Experience IdExperience { get; set; }
        public virtual Grade Grade { get; set; }
    }
}
