using System.Collections.ObjectModel;
using System.Diagnostics;
using PersonalManager.Config;
using PersonalManager.Models;

namespace PersonalManager.Views
{
    public partial class EventDetailsPage : ContentPage
    {
        private readonly AppDbContext _dbContext;
        private int _currentEventId;
        private string _selectedColor;
        public ObservableCollection<Color> Colors { get; set; }

        public EventDetailsPage(int eventId)
        {
            InitializeComponent();
            _dbContext = new AppDbContext("personal_manager.db");
            _currentEventId = eventId;
            LoadEventDetails(eventId);
            Colors = new ObservableCollection<Color>
    {
        Color.FromArgb("#007bff"), // Niebieski
        Color.FromArgb("#28a745"), // Zielony
        Color.FromArgb("#ffc107"), // ¯ó³ty
        Color.FromArgb("#dc3545"), // Czerwony
        Color.FromArgb("#6c757d"), // Szary
        Color.FromArgb("#17a2b8")  // Turkusowy
    };

            BindingContext = this;

        }

        private readonly Dictionary<string, string> _colorNames = new()
{
    { "#007bff", "Niebieski" },
    { "#28a745", "Zielony" },
    { "#ffc107", "¯ó³ty" },
    { "#dc3545", "Czerwony" },
    { "#6c757d", "Szary" },
    { "#17a2b8", "Turkusowy" }
};

        private async Task LoadEventDetails(int eventId)
        {
            var eventDetails = await _dbContext.FindAsync<Event>(eventId);

            if (eventDetails != null)
            {
                TitleLabel.Text = eventDetails.Title;
                DescriptionLabel.Text = string.IsNullOrEmpty(eventDetails.Description) ? "Brak opisu" : eventDetails.Description;
                CategoryLabel.Text = string.IsNullOrEmpty(eventDetails.Category) ? "Brak kategorii" : eventDetails.Category;
                StartDateLabel.Text = eventDetails.StartDate.ToString("f");
                EndDateLabel.Text = eventDetails.EndDate.ToString("f");

                try
                {
                    ColorFrame.BackgroundColor = Color.FromArgb(eventDetails.Color);
                }
                catch
                {
                    ColorFrame.BackgroundColor = Color.FromArgb("#808080"); 
                }
                ObservableCollection<string> Categories = new ObservableCollection<string>
        {
            "Spotkanie",
            "Praca",
            "Rodzina",
            "Urodziny",
            "Wakacje",
            "Zakupy",
            "Sport",
            "Rozrywka",
            "Inne"
        };

                CategoryPicker.ItemsSource = Categories;
                CategoryPicker.SelectedItem = eventDetails.Category; 
                TitleEntry.Text = eventDetails.Title;
                DescriptionEditor.Text = eventDetails.Description;
                StartDatePicker.Date = eventDetails.StartDate.Date;
                EndDatePicker.Date = eventDetails.EndDate.Date;
                StartTimePicker.Time = eventDetails.StartDate.TimeOfDay;
                EndTimePicker.Time = eventDetails.EndDate.TimeOfDay;
                _selectedColor = eventDetails.Color;
            }
            else
            {
                await DisplayAlert("B³¹d", "Nie znaleziono szczegó³ów wydarzenia", "OK");
            }
        }



        private void OnUpdateClicked(object sender, EventArgs e)
        {
            TitleLabel.IsVisible = false;
            DescriptionLabel.IsVisible = false;
            CategoryLabel.IsVisible = false;
            StartDateLabel.IsVisible = false;
            EndDateLabel.IsVisible = false;
            ColorFrame.IsVisible = false;

            TitleEntry.IsVisible = true;
            DescriptionEditor.IsVisible = true;
            CategoryPicker.IsVisible = true;
            StartDatePicker.IsVisible = true;
            StartTimePicker.IsVisible = true;
            EndDatePicker.IsVisible = true;
            EndTimePicker.IsVisible = true;

            TytulLabel.IsVisible = false;
            OpisLabel.IsVisible = false;
            KategoriaLabel.IsVisible = false;
            DataRozpoczeciaLabel.IsVisible = false;
            DataZakonczeniaLabel.IsVisible = false;
            KolorLabel.IsVisible = false;

            ColorCollection.IsVisible = true;
            SelectedColorLabel.IsVisible = true;

            UpdateButton.IsVisible = false; 
            SaveButton.IsVisible = true;   
            CancelButton.IsVisible = true; 
        }


        private async void OnSaveClicked(object sender, EventArgs e)
        {

            var title = TitleEntry.Text;
            var description = DescriptionEditor.Text;
            var category = CategoryPicker.SelectedItem?.ToString();
            var startDate = StartDatePicker.Date + StartTimePicker.Time;
            var endDate = EndDatePicker.Date + EndTimePicker.Time;

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) ||
                string.IsNullOrEmpty(category) || string.IsNullOrEmpty(_selectedColor))
            {
                await DisplayAlert("B³¹d", "Wszystkie pola musz¹ byæ wype³nione.", "OK");
                return;
            }

            if (startDate >= endDate)
            {
                await DisplayAlert("B³¹d", "Data rozpoczêcia musi byæ wczeœniejsza ni¿ data zakoñczenia.", "OK");
                return;
            }

            var updatedEvent = new Event
            {
                Id = _currentEventId,
                Title = title,
                Description = description,
                Category = category,
                StartDate = startDate,
                EndDate = endDate,
                Color = _selectedColor
            };

            try
            {
                await _dbContext.UpdateAsync(updatedEvent);
                await DisplayAlert("Sukces", "Wydarzenie zosta³o zaktualizowane.", "OK");
                await LoadEventDetails(_currentEventId);
            }
            catch (Exception ex)
            {
                await DisplayAlert("B³¹d", "Wyst¹pi³ b³¹d podczas zapisywania zmian.", "OK");
            }


            TitleLabel.IsVisible = true;
            DescriptionLabel.IsVisible = true;
            CategoryLabel.IsVisible = true;
            StartDateLabel.IsVisible = true;
            EndDateLabel.IsVisible = true;
            ColorFrame.IsVisible = true;

            TitleEntry.IsVisible = false;
            DescriptionEditor.IsVisible = false;
            CategoryPicker.IsVisible = false;
            StartDatePicker.IsVisible = false;
            StartTimePicker.IsVisible = false;
            EndDatePicker.IsVisible = false;
            EndTimePicker.IsVisible = false;

            TytulLabel.IsVisible = true;
            OpisLabel.IsVisible = true;
            KategoriaLabel.IsVisible = true;
            DataRozpoczeciaLabel.IsVisible = true;
            DataZakonczeniaLabel.IsVisible = true;
            KolorLabel.IsVisible = true;

            ColorCollection.IsVisible = false;
            SelectedColorLabel.IsVisible = false;

            UpdateButton.IsVisible = true; 
            SaveButton.IsVisible = false;  
            CancelButton.IsVisible = false;
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            TitleLabel.IsVisible = true;
            DescriptionLabel.IsVisible = true;
            CategoryLabel.IsVisible = true;
            StartDateLabel.IsVisible = true;
            EndDateLabel.IsVisible = true;
            ColorFrame.IsVisible = true;

            TitleEntry.IsVisible = false;
            DescriptionEditor.IsVisible = false;
            CategoryPicker.IsVisible = false;
            StartDatePicker.IsVisible = false;
            StartTimePicker.IsVisible = false;
            EndDatePicker.IsVisible = false;
            EndTimePicker.IsVisible = false;

            TytulLabel.IsVisible = true;
            OpisLabel.IsVisible = true;
            KategoriaLabel.IsVisible = true;
            DataRozpoczeciaLabel.IsVisible = true;
            DataZakonczeniaLabel.IsVisible = true;
            KolorLabel.IsVisible = true;

          
            ColorCollection.IsVisible = false;
            SelectedColorLabel.IsVisible = false;

            UpdateButton.IsVisible = true; 
            SaveButton.IsVisible = false;  
            CancelButton.IsVisible = false;
        }
      
        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usun¹æ to wydarzenie?", "Tak", "Nie");
            if (confirm)
            {
                try
                {
                    var eventDetails = await _dbContext.FindAsync<Event>(_currentEventId);
                    if (eventDetails != null)
                    {
                        await _dbContext.DeleteAsync<Event>(_currentEventId); 
                        await DisplayAlert("Sukces", "Wydarzenie zosta³o usuniête.", "OK");
                        await Navigation.PopAsync(); 
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"B³¹d podczas usuwania wydarzenia: {ex.Message}");
                    await DisplayAlert("B³¹d", "Nie uda³o siê usun¹æ wydarzenia.", "OK");
                }
            }
        }
        private void OnColorSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Color selectedColor)
            {
                _selectedColor = selectedColor.ToHex().ToLowerInvariant();
                if (_colorNames.TryGetValue(_selectedColor, out var colorName))
                {
                    SelectedColorLabel.Text = $"Wybrano kolor: {colorName}";
                }
                else
                {
                    SelectedColorLabel.Text = $"Wybrano kolor: {_selectedColor}";
                }
            }
        }
        private async void OnAddNoteClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddNotePage(_currentEventId));
        }
        private async void OnAddChecklistClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChecklistPage(_currentEventId));
        }
    }
}