using System.Collections.ObjectModel;
using System.Diagnostics;
using PersonalManager.Config;
using PersonalManager.Models;

namespace PersonalManager.Views;

public partial class AddEventPage : ContentPage
{
    private readonly AppDbContext _dbContext;
    private string _selectedColor;

    public ObservableCollection<Color> Colors { get; set; }
    public ObservableCollection<string> Categories { get; set; }



    public AddEventPage()
    {
        InitializeComponent();
        _dbContext = new AppDbContext("personal_manager.db");
        Colors = new ObservableCollection<Color>
        {
            Color.FromArgb("#007bff"), // Niebieski
            Color.FromArgb("#28a745"), // Zielony
            Color.FromArgb("#ffc107"), // ¯ó³ty
            Color.FromArgb("#dc3545"), // Czerwony
            Color.FromArgb("#6c757d"), // Szary
            Color.FromArgb("#17a2b8")  // Turkusowy
        };

        Categories = new ObservableCollection<string>
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


    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            var title = TitleEntry.Text?.Trim();
            var description = DescriptionEditor.Text?.Trim();
            var startDate = StartDatePicker.Date.Add(StartTimePicker.Time);
            var endDate = EndDatePicker.Date.Add(EndTimePicker.Time);
            var category = CategoryPicker.SelectedItem?.ToString();

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

            var newEvent = new Event
            {
                Title = title,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
                Category = category,
                Color = _selectedColor
            };

            await _dbContext.InsertAsync(newEvent);
            await Navigation.PopAsync(); 
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"B³¹d podczas zapisywania wydarzenia: {ex.Message}");
            await DisplayAlert("B³¹d", "Wyst¹pi³ problem podczas zapisywania wydarzenia.", "OK");
        }
    }
}
