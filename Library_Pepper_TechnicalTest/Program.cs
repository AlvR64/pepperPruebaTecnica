using LibraryPepper.API;
using LibraryPepper.API.Middlewares;
using LibraryPepper.Domain.Entities;
using LibraryPepper.Infrastructure.Context;
using Mapster;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInMemoryDbContext(builder.Configuration);

using (var serviceProvider = builder.Services.BuildServiceProvider())
{
    // Obtener el contexto de la base de datos
    var dbContext = serviceProvider.GetRequiredService<LibraryContext>();

    // Garantizar que la base de datos está creada
    dbContext.Database.EnsureCreated();

    // Agregar datos iniciales si la tabla está vacía
    if (!dbContext.Books.Any())
    {
        dbContext.Books.AddRange(
            new Book { Id = 1, Title = "Pepper", PublicationDate = new DateTime(2001, 1, 1), AuthorName = "Author 1" },
            new Book { Id = 2, Title = "Quijote", PublicationDate = new DateTime(2002, 2, 2), AuthorName = "Author 2" },
            new Book { Id = 3, Title = "Lord of the Rings", PublicationDate = new DateTime(2003, 3, 3), AuthorName = "Author 3" }
        );

        dbContext.SaveChanges();
    }
}

builder.Services.AddAppServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
    
app.UseMiddleware<ErrorsMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.Map("/", async (context) => //redirect home to swawgger
    {
        context.Response.Redirect("/swagger");
        await context.Response.CompleteAsync();
    });
});

app.Run();
