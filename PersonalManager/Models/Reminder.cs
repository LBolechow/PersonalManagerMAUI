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
        public int Id { get; set; } // Unikalny identyfikator przypomnienia
        public DateTime ReminderDate { get; set; } // Data i godzina przypomnienia
        public string Frequency { get; set; } // Częstotliwość (np. codziennie, co tydzień)

        public int? EventId { get; set; } // Klucz obcy wskazujący na wydarzenie
        public Event Event { get; set; } // Obiekt wydarzenia (jeśli jest powiązane)

        public int? MyTaskId { get; set; } // Klucz obcy wskazujący na zadanie
        public MyTask MyTask { get; set; } // Obiekt zadania (jeśli jest powiązane)
    }
}
