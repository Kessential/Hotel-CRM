// Composition Root of the web app. For now it only wires Razor Pages; the BLL/DAL objects are
// created here by hand (new ...) once they move to SQLite.
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.Extensions.WebEncoders;

// The content root is the program's own folder, not the current directory, so appsettings.json and
// wwwroot are still found when the published app is started from somewhere else.
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory,
});
// The build output has no wwwroot of its own: this reads the manifest that points to the source files.
// ASP.NET Core only does it by itself in Development; a published app has no manifest and is not affected.
builder.WebHost.UseStaticWebAssets();

// Razor escapes every non-ASCII letter as &#x...; by default. Allowing all Unicode ranges keeps
// Vietnamese text readable in the page source; <, >, & and quotes are still escaped (XSS stays blocked).
builder.Services.Configure<WebEncoderOptions>(options =>
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

// "/" opens the home page; every other page keeps its frm name as its URL (/frm201_dm_customer).
builder.Services.AddRazorPages(options => options.Conventions.AddPageRoute("/frm001_main", ""));

var app = builder.Build();

app.UseRouting();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
