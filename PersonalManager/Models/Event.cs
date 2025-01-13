using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalManager.Models
{
    public class Event
    {
        public int Id { get; set; } // Unikalny identyfikator wydarzenia

        public string Title { get; set; } // Tytuł wydarzenia
        public string Description { get; set; } // Opis wydarzenia
        public DateTime StartDate { get; set; } // Data i godzina rozpoczęcia
        public DateTime EndDate { get; set; } // Data i godzina zakończenia
        public string Category { get; set; } // Kategoria wydarzenia (np. praca, hobby)
        public string Color { get; set; } // Kolor dla wydarzenia, np. z kodem hex
    }
}
