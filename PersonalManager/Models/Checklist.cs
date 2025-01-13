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
        public int Id { get; set; } // Unikalny identyfikator checklisty
        public string Title { get; set; } // Tytuł checklisty (np. "Zakupy", "Rzeczy do zrobienia")

        public int? EventId { get; set; } // Klucz obcy wskazujący na wydarzenie
        public Event Event { get; set; } // Powiązane wydarzenie (jeśli istnieje)

        public int? MyTaskId { get; set; } // Klucz obcy wskazujący na zadanie
        public MyTask MyTask { get; set; } // Powiązane zadanie (jeśli istnieje)

    }
}
