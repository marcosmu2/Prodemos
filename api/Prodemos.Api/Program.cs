using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Prodemos.Application.Models.Email;
using Prodemos.Application.Services.Interfaces;
using Prodemos.Domain;
using Prodemos.Infrastructure.Persistence;
using Prodemos.Infrastructure.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ProdemosDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly(typeof(ProdemosDbContext).Assembly.FullName)
    )
);

builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});

IdentityBuilder identityBuilder = builder.Services.AddIdentityCore<User>();
identityBuilder = new IdentityBuilder(identityBuilder.UserType, identityBuilder.Services);

identityBuilder.AddRoles<IdentityRole>().AddDefaultTokenProviders();
identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<User, IdentityRole>>();

identityBuilder.AddEntityFrameworkStores<ProdemosDbContext>();
identityBuilder.AddSignInManager<SignInManager<User>>();

builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateAudience = false,
        ValidateIssuer = false
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", builder => builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    );
});

builder.Services.AddTransient<IManageImageService, ManageImageService>();
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.Configure<EmailFluentSettings>(builder.Configuration.GetSection(nameof(EmailFluentSettings)));
var emailSettings = builder.Configuration.GetSection(nameof(EmailFluentSettings));
var fromEmail = emailSettings.GetValue<String>("Email");
var host = emailSettings.GetValue<String>("Host");
var port = emailSettings.GetValue<int>("Port");
builder.Services.AddFluentEmail(fromEmail).AddSmtpSender(host,port);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("NewCors");

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var loggerFactory = service.GetRequiredService<ILoggerFactory>();

    try
    {
        var context = service.GetRequiredService<ProdemosDbContext>();
        var userManager = service.GetRequiredService<UserManager<User>>();
        var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
        await context.Database.MigrateAsync();
        await ProdemosDbContextData.LoadDataASync(context, userManager, roleManager, loggerFactory);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Error en la migration");
    }
}

app.Run();
