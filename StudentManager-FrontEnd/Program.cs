using System.Net;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
{
    // Configure session options here
    options.Cookie.Name = ".MySession";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true; // Ensure session cookie is HTTP-only
    // Add other session options as needed
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});
builder.Services.AddHttpContextAccessor();

// Add HttpClient
builder.Services.AddHttpClient("YourHttpClient", client =>
{
    client.BaseAddress = new Uri("https://your-api-url.com");
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        UseCookies = true,
        UseDefaultCredentials = false,
        CookieContainer = new CookieContainer(),
        AllowAutoRedirect = true
    };
});

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

// Use session middleware
app.UseSession();

// Use CORS policy
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapRazorPages();

app.Run();
