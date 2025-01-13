using System.Collections.ObjectModel;
using System.Windows.Input;
using PersonalManager.Models;
using PersonalManager.Config;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace PersonalManager.Views
{
    public partial class ChecklistPage : ContentPage, INotifyPropertyChanged
    {
        private readonly AppDbContext _dbContext;
        private int _eventId;

        public Checklist Checklist { get; private set; }
        public ObservableCollection<CheckItem> CheckItems { get; set; } // Zmieniona lista na CheckItems
        public string NewItemContent { get; set; }

        public ICommand AddCheckItemCommand { get; }
        public ICommand DeleteCheckItemCommand { get; }
        public ICommand SaveChecklistCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ChecklistPage(int eventId)
        {
            InitializeComponent();
            _eventId = eventId;
            _dbContext = new AppDbContext("personal_manager.db");
           

            // Inicjalizacja komend
            AddCheckItemCommand = new Command(AddCheckItem);
            DeleteCheckItemCommand = new Command<int>(DeleteCheckItem);
            SaveChecklistCommand = new Command(SaveChecklist);
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadChecklist(_eventId);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // £adowanie checklisty i elementów
        private async Task LoadChecklist(int eventId)
        {
            Checklist = await _dbContext.GetChecklistWithItemsAsync(eventId) ?? new Checklist { EventId = eventId };
            CheckItems = new ObservableCollection<CheckItem>(await _dbContext.GetCheckItemsByChecklistIdAsync(Checklist.Id));
            OnPropertyChanged(nameof(Checklist));
            OnPropertyChanged(nameof(CheckItems));
        }

        // Dodawanie nowego elementu do checklisty
        private void AddCheckItem()
        {
            if (!string.IsNullOrWhiteSpace(NewItemContent))
            {
                var newItem = new CheckItem { Content = NewItemContent, IsCompleted = false, ChecklistId = Checklist.Id };
                CheckItems.Add(newItem);
                NewItemContent = string.Empty;
                OnPropertyChanged(nameof(NewItemContent));
            }
        }

        // Usuwanie elementu z checklisty
        private async void DeleteCheckItem(int id)
        {
            var item = CheckItems.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                CheckItems.Remove(item);
                await _dbContext.DeleteCheckItemAsync(id);
            }
        }

        // Zapisanie checklisty
        private async void SaveChecklist()
        {
            System.Diagnostics.Debug.WriteLine("SaveChecklistCommand called");

            if (Checklist.Id == 0)
            {
                await _dbContext.AddChecklistAsync(Checklist);
            }
            else
            {
                await _dbContext.UpdateChecklistAsync(Checklist);
            }

            foreach (var item in CheckItems)
            {
                if (item.Id == 0)
                {
                    item.ChecklistId = Checklist.Id;
                    await _dbContext.AddCheckItemAsync(item);
                }
                else
                {
                    await _dbContext.UpdateCheckItemAsync(item);
                }
            }

            // Po zapisaniu checklisty wyœwietlamy komunikat
            await DisplayAlert("Sukces", "Checklisty zosta³y zapisane pomyœlnie.", "OK");
        }

    }
}
