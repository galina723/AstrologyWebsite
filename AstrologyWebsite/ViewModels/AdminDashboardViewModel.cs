namespace AstrologyWebsite.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalBookings { get; set; }
        public int ActiveReaders { get; set; }
        public int TotalUsers { get; set; }
        public int NewUsers { get; set; }
        public List<BookingStatusCount> BookingStatus { get; set; }
        public List<UpcomingBooking> UpcomingBookings { get; set; }
        public List<PopularService> PopularServices { get; set; }
        public List<int> BookingTrends { get; set; } // e.g. bookings per month
    }
}
