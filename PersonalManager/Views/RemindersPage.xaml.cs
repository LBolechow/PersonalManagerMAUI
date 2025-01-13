using PersonalManager.Config;
using PersonalManager.Models;
using System.Collections.ObjectModel;

namespace PersonalManager.Views
{
    public partial class RemindersPage : ContentPage
    {
        private readonly AppDbContext _dbContext;
        public ObservableCollection<Event> Events { get; set; }

        public RemindersPage()
        {
            InitializeComponent();
            _dbContext = new AppDbContext("personal_manager.db");
            LoadEvents();
        }

        private async void LoadEvents()
        {
            try
            {
                var eventsFromDb = await _dbContext.GetAllAsync<Event>();
                Events = new ObservableCollection<Event>(eventsFromDb);

                EventsListView.ItemsSource = Events; // Przypisanie danych do ListView
            }
            catch (Exception ex)
            {
                await DisplayAlert("B³¹d", $"Nie uda³o siê za³adowaæ wydarzeñ: {ex.Message}", "OK");
            }
        }
    }
}
