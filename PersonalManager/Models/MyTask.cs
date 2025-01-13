using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalManager.Models
{
    public class MyTask
    {
        public int Id { get; set; } // Unikalny identyfikator zadania
        public string Title { get; set; } // Tytuł zadania
        public string Description { get; set; } // Opis zadania
        public DateTime StartDate { get; set; } // Data i godzina rozpoczęcia
        public DateTime EndDate { get; set; } // Data i godzina zakończenia
        public string Priority { get; set; } // Priorytet zadania (np. niski, średni, wysoki)
        public string Status { get; set; } // Status zadania (np. "Do zrobienia", "W trakcie", "Zrobione")
        public string Category { get; set; } // Kategoria zadania (np. praca, hobby)
    }
}
