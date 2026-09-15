using Microsoft.AspNetCore.Identity;
using TimeSlot.Models;
namespace TimeSlot.Data;
// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public List<Booking>? Bookings { get; set; }
}
