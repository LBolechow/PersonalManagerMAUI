using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace PersonalManager.Models
{
    public class CheckItem
    {
        public int Id { get; set; } 
        public string Content { get; set; } 
        public bool IsCompleted { get; set; } 

        public int ChecklistId { get; set; }
        public Checklist Checklist { get; set; }
    }
}
