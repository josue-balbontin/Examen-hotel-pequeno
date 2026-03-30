using Microsoft.EntityFrameworkCore;
using Backend.Modelos;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseNpgsql("Host=localhost;Database=hotel_pequeno;Username=postgres;Password=273153"));


// 2. INYECCIÓN DE DEPENDENCIAS (Aquí registraremos tus Servicios y Repositorios más adelante)
// Ejemplo: builder.Services.AddScoped<HabitacionService>();
// Ejemplo: builder.Services.AddScoped<HabitacionRepository>();


builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();           

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseHttpsRedirection();


app.MapControllers();

Console.WriteLine("Swagger URL: http://localhost:5052/swagger/index.html");

app.Run();