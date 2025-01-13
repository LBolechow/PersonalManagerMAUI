namespace PersonalManager.Views
{
    using PersonalManager.Config; // Importujemy AppDbContext
    using System.Diagnostics;
    using System.Linq;

    public partial class ToDoPage : ContentPage
    {
        private readonly AppDbContext _dbContext;

        public ToDoPage()
        {
            InitializeComponent();
            _dbContext = new AppDbContext("personal_manager.db"); // Tworzymy obiekt AppDbContext
            LoadTableNames();
        }

        private async void LoadTableNames()
        {
            // Pobierz listê nazw tabel
            var tableNames = await _dbContext.GetAllTableNamesAsync(); // Zak³adaj¹c, ¿e masz metodê GetAllTableNamesAsync w AppDbContext

            // Ustaw listê danych w ListView
            TableNamesListView.ItemsSource = tableNames;
        }

        private async void OnTableSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Odczytanie wybranej tabeli
            var selectedTable = e.SelectedItem as string;

            // Mo¿esz teraz zrobiæ coœ z wybran¹ tabel¹ (np. pobraæ dane z tej tabeli)
            Debug.WriteLine($"Selected table: {selectedTable}");
        }
    }
}
