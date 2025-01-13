using System.Collections.ObjectModel;
using System.Diagnostics;
using PersonalManager.Config;
using PersonalManager.Models;

namespace PersonalManager.Views;

public partial class AddNotePage : ContentPage
{
    private int _eventId;
    private readonly AppDbContext _dbContext;

    public ObservableCollection<Note> Notes { get; set; }

    public AddNotePage(int eventId)
    {
        InitializeComponent();
        _eventId = eventId;
        _dbContext = new AppDbContext("personal_manager.db");

        // Inicjalizowanie listy notatek
        Notes = new ObservableCollection<Note>();
        BindingContext = this;

        LoadNotes();
    }

    private async void LoadNotes()
    {
        try
        {
            Notes.Clear();
            // Pobierz notatki zwi�zane z danym wydarzeniem
            var notes = await _dbContext.GetNotesByEventIdAsync(_eventId);
            foreach (var note in notes)
            {
                Notes.Add(note); // Dodaj notatki do kolekcji
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"B��d podczas �adowania notatek: {ex.Message}");
            await DisplayAlert("B��d", "Nie uda�o si� za�adowa� notatek.", "OK");
        }
    }

    private async void OnAddNoteClicked(object sender, EventArgs e)
    {
        // Wy�wietl modalne okno do wpisania tre�ci notatki
        var result = await DisplayPromptAsync("Nowa Notatka", "Wpisz tre�� notatki:");

        // Sprawd�, czy u�ytkownik wpisa� tre��
        if (!string.IsNullOrWhiteSpace(result))
        {
            var note = new Note
            {
                Content = result,
                EventId = _eventId // Tylko identyfikator wydarzenia
            };

            // Dodaj notatk� do bazy danych
            await _dbContext.AddNoteAsync(note);

            // Od�wie�enie listy notatek
            LoadNotes();
        }
        else
        {
            await DisplayAlert("B��d", "Tre�� notatki nie mo�e by� pusta.", "OK");
        }
    }

    private async void OnEditNoteClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var noteId = (int)button.CommandParameter;

        var note = await _dbContext.GetByIdAsync<Note>(noteId);
        if (note != null)
        {
            var result = await DisplayPromptAsync("Edytuj Notatk�", "Wpisz now� tre�� notatki:", initialValue: note.Content);

            if (!string.IsNullOrWhiteSpace(result))
            {
                note.Content = result;

                // Aktualizowanie notatki w bazie danych
                await _dbContext.UpdateNoteAsync(note);

                // Od�wie�enie listy notatek
                LoadNotes();
            }
            else
            {
                await DisplayAlert("B��d", "Tre�� notatki nie mo�e by� pusta.", "OK");
            }
        }
    }

    private async void OnDeleteNoteClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var noteId = (int)button.CommandParameter;

        var note = await _dbContext.GetByIdAsync<Note>(noteId);
        if (note != null)
        {
            var confirmDelete = await DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usun�� t� notatk�?", "Tak", "Nie");

            if (confirmDelete)
            {
                // Usuni�cie notatki z bazy danych
                await _dbContext.DeleteAsync<Note>(noteId);

                // Od�wie�enie listy notatek
                LoadNotes();
            }
        }
    }
}
