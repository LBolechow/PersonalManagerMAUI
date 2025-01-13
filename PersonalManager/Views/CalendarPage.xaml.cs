using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using PersonalManager.Config;
using PersonalManager.Models;
using System.Diagnostics;
using System.Globalization;

namespace PersonalManager.Views;

public partial class CalendarPage : ContentPage
{
    public ObservableCollection<SchedulerAppointment> Events { get; set; }

    private readonly AppDbContext _dbContext;
    public CalendarPage()
	{
		InitializeComponent();
        _dbContext = new AppDbContext("personal_manager.db");
        var culture = new CultureInfo("pl-PL");
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        if (_dbContext == null)
        {
            Debug.WriteLine("DbContext is null!");
        }
       LoadEventsFromDatabase();
    }
    private async void OnAddEventClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddEventPage());
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadEventsFromDatabase(); 
    }
    private async void OnSchedulerTapped(object sender, SchedulerTappedEventArgs e)
    {
        if (e.Appointments != null && e.Appointments.FirstOrDefault() is SchedulerAppointment appointment)
        {
            var eventFromDb = await _dbContext.GetAllAsync<Event>();
            var eventId = eventFromDb.FirstOrDefault(ev =>
                ev.Title == appointment.Subject &&
                ev.StartDate == appointment.StartTime &&
                ev.EndDate == appointment.EndTime)?.Id;

            if (eventId != null)
            {
                await Navigation.PushAsync(new EventDetailsPage(eventId.Value));
            }
        }
    }
    private async Task LoadEventsFromDatabase()
    {
        try
        {
            var events = await _dbContext.GetAllAsync<Event>();

            if (events == null || !events.Any())
            {
                Debug.WriteLine("Baza danych jest pusta! Tworzê predefiniowane wydarzenie.");

                var defaultEvent = new SchedulerAppointment
                {
                    Subject = "Predefiniowane wydarzenie",
                    StartTime = DateTime.Now.AddHours(1),
                    EndTime = DateTime.Now.AddHours(2),
                    Background = Color.FromArgb("#007bff"),
                    Location = "Brak kategorii"
                };

                Events = new ObservableCollection<SchedulerAppointment> { defaultEvent };
                EventScheduler.AppointmentsSource = Events;
                return;
            }
            Events = new ObservableCollection<SchedulerAppointment>(
                events.Select(e =>
                {
                    Color backgroundColor;
                    try
                    {
                        backgroundColor = Color.FromArgb(e.Color);
                    }
                    catch
                    {
                        Debug.WriteLine($"Nieprawid³owy kolor: {e.Color}. Ustawiam domyœlny kolor.");
                        backgroundColor = Color.FromArgb("#6c757d"); 
                    }

                    return new SchedulerAppointment
                    {
                        Subject = e.Title,
                        StartTime = e.StartDate,
                        EndTime = e.EndDate,
                        Background = backgroundColor,
                        Location = e.Category
                    };
                })
            );

            EventScheduler.AppointmentsSource = Events;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"B³¹d podczas ³adowania danych z bazy: {ex.Message}");
        }
    }


}