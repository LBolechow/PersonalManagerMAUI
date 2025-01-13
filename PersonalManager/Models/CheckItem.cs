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
        public int Id { get; set; } // Unikalny identyfikator elementu
        public string Content { get; set; } // Treść elementu
        public bool IsCompleted { get; set; } // Status ukończenia

        public int ChecklistId { get; set; } // Klucz obcy do checklisty
        public Checklist Checklist { get; set; } // Powiązana checklista
    }
}
