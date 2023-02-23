
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YourMobile.DataAccess.Data;
using YourMobile.DataAccess.DbInitializer;
using YourMobile.DataAccess.IRepository;
using YourMobile.DataAccess.Repository;
using Stripe;
using YourMobile.DataAccess.Utility;
using Abby.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();





var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
	   connectionString: builder.Configuration.GetConnectionString("DefaultConnection")
	));


//itt is módosítani kell a Role jogok miatt
builder.Services.AddIdentity<IdentityUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders()

	;

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
builder.Services.AddScoped<IPhotoRepository, PhotoRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddScoped<IDbInitializer,DbInitializer>();

builder.Services.Configure<StripePaymentSettings>(builder.Configuration.GetSection("Stripe"));



builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Identity/Account/Login";
	options.LogoutPath = "/Identity/Account/Logout";
	options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");

	
	
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
else
{
	app.UseExceptionHandler("/Error");
	app.Use(async (ctx, next) => {
		await next();
		if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
		{
			ctx.Request.Path = "/Pages404";
			await next();
		}
	});
}






app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();

SeedDatabase();

string key = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
StripeConfiguration.ApiKey = key;

app.Run();




void SeedDatabase()
{
	using (var scope = app.Services.CreateScope())
	{
		var dbInit = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
		dbInit.Initialize();
	}
}