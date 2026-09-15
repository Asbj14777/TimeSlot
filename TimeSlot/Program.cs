using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
using TimeSlot.Data;
using TimeSlot.Interfaces;
using TimeSlot.Persistence;
using TimeSlot.Services;
using Microsoft.Identity;
using Microsoft.AspNetCore.Identity;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TimeSlotContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("default")); });

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<TimeSlotContext>();
// Add services to the container.
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Bookings}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
