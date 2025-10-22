using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalManager.Models
{
    public class Reminder
    {
        public int Id { get; set; }
        public DateTime ReminderDate { get; set; } 
        public string Frequency { get; set; } 

        public int? EventId { get; set; } 
        public Event Event { get; set; }

        public int? MyTaskId { get; set; } 
        public MyTask MyTask { get; set; }
    }
}
