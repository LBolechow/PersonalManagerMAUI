using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace PersonalManager.Models
{
    public class Checklist
    {
        public int Id { get; set; } 
        public string Title { get; set; } 

        public int? EventId { get; set; } 
        public Event Event { get; set; } 

        public int? MyTaskId { get; set; }
        public MyTask MyTask { get; set; } 

    }
}
