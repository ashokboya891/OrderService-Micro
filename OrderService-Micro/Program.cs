using OrderService_Micro;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

// Register Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(builder.Configuration["Redis:ConnectionString"])
);

// ✅ Configure CORS properly
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:4200") // ✅ Replace with your frontend's exact URL
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // ✅ This requires a specific origin, not "*"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting(); // ✅ Ensure routing middleware is used before mapping controllers

app.UseCors("CorsPolicy"); // ✅ Apply the CORS policy

app.UseAuthorization();

// ✅ Map the SignalR Hub for real-time updates
app.MapHub<OrderHub>("/orderHub");

app.MapControllers();

app.Run();
