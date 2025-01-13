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
        public int Id { get; set; } // Unikalny identyfikator notatki
        public string Content { get; set; } // Zawartość notatki

        public int? EventId { get; set; } // Klucz obcy wskazujący na wydarzenie
        public Event Event { get; set; } // Obiekt wydarzenia (jeśli jest powiązane)

        public int? MyTaskId { get; set; } // Klucz obcy wskazujący na zadanie
        public MyTask MyTask { get; set; } // Obiekt zadania (jeśli jest powiązane)
    }
}
