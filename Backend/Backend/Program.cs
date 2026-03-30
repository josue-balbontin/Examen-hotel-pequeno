using Microsoft.EntityFrameworkCore;
using Backend.Modelos.Entidades;
using Backend.Repositorio.Usuario;
using Backend.Repositorio.Reserva;
using Backend.Servicios;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("PoliticaCors", app =>
    {
        app.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});



builder.Services.AddControllers();

builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseNpgsql("Host=localhost;Database=hotel_pequeno;Username=postgres;Password=273153"));


builder.Services.AddScoped<IUsuarioRepositorio , UsuarioRepositorio>();
builder.Services.AddScoped<IUsuarioServicio , UsuarioServicio>();
builder.Services.AddScoped<IReservaRepositorio , ReservaRepositorio>();
builder.Services.AddScoped<IReservaServicio , ReservaServicio>();




builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();           

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseHttpsRedirection();

app.UseCors("PoliticaCors");


app.MapControllers();

Console.WriteLine("Swagger URL: http://localhost:5052/swagger/index.html");

app.Run();