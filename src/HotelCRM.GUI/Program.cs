// Composition Root of the web app. For now it only wires Razor Pages; the BLL/DAL objects are
// created here by hand (new ...) once they move to SQLite.
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.Extensions.WebEncoders;

var builder = WebApplication.CreateBuilder(args);

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
