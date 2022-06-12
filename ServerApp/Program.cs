using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServerApp.Data;
using ServerApp.Filters;
using ServerApp.Models;
using ServerApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

string MyAllowOrigins = "_myAllowOrigins";

builder.Services.AddScoped(typeof(IRepository), typeof(Repository));

builder.Services.AddDbContext<SocialContext>(x => x.UseSqlite("Data Source=social.db"));

builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<SocialContext>();

builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequireDigit = true; // Mutlaka sayısal bir değer olsun mu ? 
    options.Password.RequireLowercase = true; // Mmtlaka küçük harf olsun mu ?
    options.Password.RequireUppercase = true; // Mutlaka büyük harf olsun mu ?
    options.Password.RequireNonAlphanumeric = true; // Mutlaka içerisinde alphanumeric harf olsun mu ?
    options.Password.RequiredLength = 8; // Parola uzunluğu kaç karakter olsun. (Default değeri 6)

    options.Lockout.MaxFailedAccessAttempts = 5; // Hesabına 5 kere yanlış şifre girer ise bu durumda 5 dakika kullanıcının hesabı kilitlenmiş olucak
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Hesap kilitlendikten sonra 5 dakika bekleticek
    options.Lockout.AllowedForNewUsers = true; // Hesabı yeni oluşturulan bir kişinin hesabının kilitlenip, kilitlenmeyeceğini belirtiyoruz.

    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; // UserName içerisinde olması gereken karakterleri burada yazıyoruz.
    options.User.RequireUniqueEmail = true; // Mutlaka her oluşturulan kullanıcının mail adresi birbirinden farklı olmalıdır. Ayni mail adresine sahip iki kullanıcı olamaz.
});

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers().AddNewtonsoftJson(options => {
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowOrigins,
        builder =>
        {
            builder
            .WithOrigins("http://localhost:4200") // Bu sayfaların izni var
            // .WithMethods("GET", "POST", "PUT") // Get, Post ve Put requestlerine izin verdik.
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme =  JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    x.RequireHttpsMetadata = false; // Sadece https protokolünü kullanan isteklerden mi gelsin ?
    x.SaveToken = true; // Token bilgisi, server tarafında kaydedilsin mi ?
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings:Secret").Value)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddScoped<LastActiveActionFilter>();


#region Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();

    using (var scope = app.Services.CreateScope())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        SeedDatabase.Seed(userManager).Wait();
    }
}
else
{
    app.UseExceptionHandler(appError => {
        appError.Run(async context => {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var exception = context.Features.Get<IExceptionHandlerFeature>();

            if (exception!=null)
            {
                // Loglama => nlog, elmah
                await context.Response.WriteAsync(new ErrorDetails()
                {
                    StatusCode = context.Response.StatusCode,
                    Message = exception.Error.Message
                }.ToString());
            }
        });
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(MyAllowOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();