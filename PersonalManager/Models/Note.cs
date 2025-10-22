using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalManager.Models
{
    public class Note
    {
        public int Id { get; set; } 
        public string Content { get; set; }

        public int? EventId { get; set; } 
        public Event Event { get; set; } 

        public int? MyTaskId { get; set; } 
        public MyTask MyTask { get; set; } 
    }
}
