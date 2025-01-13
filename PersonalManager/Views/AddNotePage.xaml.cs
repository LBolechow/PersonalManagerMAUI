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
            // Pobierz notatki zwi¹zane z danym wydarzeniem
            var notes = await _dbContext.GetNotesByEventIdAsync(_eventId);
            foreach (var note in notes)
            {
                Notes.Add(note); // Dodaj notatki do kolekcji
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"B³¹d podczas ³adowania notatek: {ex.Message}");
            await DisplayAlert("B³¹d", "Nie uda³o siê za³adowaæ notatek.", "OK");
        }
    }

    private async void OnAddNoteClicked(object sender, EventArgs e)
    {
        // Wyœwietl modalne okno do wpisania treœci notatki
        var result = await DisplayPromptAsync("Nowa Notatka", "Wpisz treœæ notatki:");

        // SprawdŸ, czy u¿ytkownik wpisa³ treœæ
        if (!string.IsNullOrWhiteSpace(result))
        {
            var note = new Note
            {
                Content = result,
                EventId = _eventId // Tylko identyfikator wydarzenia
            };

            // Dodaj notatkê do bazy danych
            await _dbContext.AddNoteAsync(note);

            // Odœwie¿enie listy notatek
            LoadNotes();
        }
        else
        {
            await DisplayAlert("B³¹d", "Treœæ notatki nie mo¿e byæ pusta.", "OK");
        }
    }

    private async void OnEditNoteClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var noteId = (int)button.CommandParameter;

        var note = await _dbContext.GetByIdAsync<Note>(noteId);
        if (note != null)
        {
            var result = await DisplayPromptAsync("Edytuj Notatkê", "Wpisz now¹ treœæ notatki:", initialValue: note.Content);

            if (!string.IsNullOrWhiteSpace(result))
            {
                note.Content = result;

                // Aktualizowanie notatki w bazie danych
                await _dbContext.UpdateNoteAsync(note);

                // Odœwie¿enie listy notatek
                LoadNotes();
            }
            else
            {
                await DisplayAlert("B³¹d", "Treœæ notatki nie mo¿e byæ pusta.", "OK");
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
            var confirmDelete = await DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usun¹æ tê notatkê?", "Tak", "Nie");

            if (confirmDelete)
            {
                // Usuniêcie notatki z bazy danych
                await _dbContext.DeleteAsync<Note>(noteId);

                // Odœwie¿enie listy notatek
                LoadNotes();
            }
        }
    }
}
