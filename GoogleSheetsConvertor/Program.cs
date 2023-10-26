using GoogleSheetsConvertor.Service.GoogleDocs;
using GoogleSheetsConvertor.Service.GoogleDrive;
using GoogleSheetsConvertor.Service.GoogleSheets;
using GoogleSheetsConvertor.Service.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(typeof(GoogleHelper));
builder.Services.AddScoped<IGoogleSheetService, GoogleSheetService>();
builder.Services.AddScoped<IGoogleDocService, GoogleDocService>();
builder.Services.AddScoped<IGoogleDriveService, GoogleDriveService>();
builder.Services.AddRazorPages();
builder.Services.AddCors();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(options => options.WithOrigins("https://localhost:44363").AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.MapRazorPages();

app.Run();
