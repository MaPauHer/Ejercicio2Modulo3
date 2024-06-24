using Microsoft.EntityFrameworkCore;
using EjercicioModulo3Clase2.Domain.Entities;
using EjercicioModulo3Clase2.Repository;
using System.Threading;

namespace EjercicioModulo3Clase2
{
    public class Program
    {
        public static void Main( string[] args )
        {

            var builder = WebApplication.CreateBuilder( args );

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Inyecci�n Dependencias
            builder.Services.AddDbContext<DBContext>(opt =>
            {
                opt.UseSqlServer("Data Source=DESKTOP-EJRGUFU\\SQLEXPRESS;Initial Catalog=ToDoListDB;Integrated Security=True;Trust Server Certificate=True"); 
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();

        }
    }
}