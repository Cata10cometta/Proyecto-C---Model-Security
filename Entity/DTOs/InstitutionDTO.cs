﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class InstitutionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }   
        public string EmailInstitution { get; set; }
        public string Department { get; set; }  
        public string Commune { get; set; } 


    }
}
