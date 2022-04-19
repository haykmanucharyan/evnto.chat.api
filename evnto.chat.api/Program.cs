using evnto.chat.api;
using evnto.chat.api.Authentication;
using evnto.chat.api.ErrorHandling;
using evnto.chat.bll;
using evnto.chat.bll.Implementations;
using evnto.chat.bll.Interfaces;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// read config and init BL config
ConfigurationBuilder configBuilder = new ConfigurationBuilder();
configBuilder.SetBasePath(Directory.GetCurrentDirectory());
configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
IConfigurationRoot cfg = configBuilder.Build();

BLConfiguration blConfig = new BLConfiguration();
blConfig.DBConnectionString = (string)cfg.GetValue(typeof(string), nameof(BLConfiguration.DBConnectionString));
blConfig.RMQConnectionString = (string)cfg.GetValue(typeof(string), nameof(BLConfiguration.RMQConnectionString));
blConfig.SaltMinSeed = (int)cfg.GetValue(typeof(int), nameof(BLConfiguration.SaltMinSeed));
blConfig.SaltRepeatMin = (int)cfg.GetValue(typeof(int), nameof(BLConfiguration.SaltRepeatMin));
blConfig.SaltRepeatMax = (int)cfg.GetValue(typeof(int), nameof(BLConfiguration.SaltRepeatMax));

// create and inject BL factory
IBLFactory factory = new BLFactory(blConfig);
builder.Services.AddSingleton<IBLFactory>(factory);

// custom authentication injection
builder.Services.AddAuthentication(options => options.DefaultScheme = Constants.EvntoAuthScheme)
    .AddScheme<EvntoAuthSchemeOptions, EvntoAuthHandler>(Constants.EvntoAuthScheme, options => { });

builder.Services.AddAuthorization(options => 
{
    options.AddPolicy(Constants.EvntoAuthScheme, policy => 
    {
        policy.RequireClaim(ClaimTypes.NameIdentifier); 
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

// exception handling
app.UseMiddleware<EvntoExceptionMiddleware>();

// web sockets
WebSocketOptions wsOptions = new WebSocketOptions()
{
    KeepAliveInterval = TimeSpan.FromSeconds(30)
};
app.UseWebSockets(wsOptions);

app.Run();
