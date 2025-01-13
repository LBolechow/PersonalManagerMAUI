using PersonalManager.Views;

namespace PersonalManager
{
    public partial class MainPage : ContentPage
    {
        

        public MainPage()
        {
            InitializeComponent();
            var calendarTapGesture = new TapGestureRecognizer();
            calendarTapGesture.Tapped += async (s, e) => await Navigation.PushAsync(new CalendarPage());
            CalendarTapped.GestureRecognizers.Add(calendarTapGesture);

            var todoTapGesture = new TapGestureRecognizer();
            todoTapGesture.Tapped += async (s, e) => await Navigation.PushAsync(new ToDoPage());
            ToDoTapped.GestureRecognizers.Add(todoTapGesture);

            var remindersTapGesture = new TapGestureRecognizer();
            remindersTapGesture.Tapped += async (s, e) => await Navigation.PushAsync(new RemindersPage());
            RemindersTapped.GestureRecognizers.Add(remindersTapGesture);

            var documentsTapGesture = new TapGestureRecognizer();
            documentsTapGesture.Tapped += async (s, e) => await Navigation.PushAsync(new DocumentsPage());
            DocumentsTapped.GestureRecognizers.Add(documentsTapGesture);
        }

       
    }

}
