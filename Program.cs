using PlagiarismSystem.Models;
using PlagiarismSystem.Services.Convert;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ConvertMatchPair>();
builder.Services.AddTransient<FoldersModel>();

var app = builder.Build();

app.UseStaticFiles();


//app.MapGet("/", () => "Hello World!");
// app.MapControllerRoute(
//     name: "Account",
//     pattern: "{area:exists}/{controller=Home}/{action=Index}");
app.MapAreaControllerRoute(
    name:"Convert",
    areaName:"Convert",
    pattern:"Convert/{controller=Convert}/{action=Create}"
);
app.MapAreaControllerRoute(
    name:"Upload",
    areaName: "Upload",
    pattern:"Upload/{controller=Upload}/{action=Upload}"
);

app.MapAreaControllerRoute(
    name:"MainPage",
    areaName: "Main",
    pattern:"Main/{controller=Main}/{action=Main}"
);

app.Run();
