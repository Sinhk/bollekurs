using Bollekurs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
var appOptions = builder.Configuration.GetRequiredSection("Application").Get<ApplicationOptions>();
builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection("Application"));


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options=>{
    options.IdleTimeout = appOptions?.CaseTimeout ?? TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly=true;
    options.Cookie.IsEssential=true;
});

builder.Services.AddSingleton<CaseManager>();

var app = builder.Build();
app.Services.GetRequiredService<CaseManager>()
    .AddCases(Cases.Haiker.Values);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
