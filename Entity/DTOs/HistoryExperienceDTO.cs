﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class HistoryExperienceDTO
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public DateTime DateTime { get; set; }
        public string TableName { get; set; }
        public int UserId { get; set; }
        public int ExperienceId { get; set; }


    }
}
