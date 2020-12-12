using Xunit;

namespace POB.CALENDAR
{
    public class MyCalendarTest
    {
        [Fact]
        public async void Create()
        {
            //Given
            var graphServiceClient = MyCalendar.Create();
            var newCalendar = await graphServiceClient.Me.Calendars
                .Request()
                .AddAsync(new Microsoft.Graph.Calendar { Name = "Keila_Bitch" });
        }
    }
}