using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebSearch.Core.Services.Models;
using WebSearch.Core.Services;
using WebSearch.DataAccess;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMvc();
builder.Services.AddMvcCore()
    .AddApiExplorer()
    .AddAuthorization(options =>
    {
        options.AddPolicy("BasicAuthentication", policy =>
        {
            policy.RequireAuthenticatedUser();
        });
    });
builder.Services.AddDbContext<WebSearchDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebSearchConnection")));


// Register ISearchService and its implementation
builder.Services.AddScoped<ISearchService, SearchService>();

// Register ICreateSearchRequest and its implementations
builder.Services.AddScoped<ICreateSearchRequest, CreateGoogleSearchRequest>();
builder.Services.AddScoped<ICreateSearchRequest, CreateBingSearchRequest>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c =>
{
    /*
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Enter your username and password in the format: username:password"
    });*/
    /*
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[] { }
        }
    });*/
  
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors(); // Enable CORS

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WebSearchDbContext>();
    try
    {
        dbContext.Database.Migrate(); // Apply pending migrations
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while applying migrations.");
    }
}

//app.UseMiddleware<WebSearchApi.Middleware.BasicAuthenticationMiddleware>();

app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.UseAuthorization();

app.MapControllers();

app.Run();

