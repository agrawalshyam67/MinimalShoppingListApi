using Microsoft.EntityFrameworkCore;
using MinimalShoppingListApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiDbContext>(options => options.UseInMemoryDatabase("MinimalShoppingListApi"));

var app = builder.Build();

app.MapGet("/shoppingList", async (ApiDbContext db) => await db.Groceries.ToListAsync());

app.MapPost("/shoppingList", async (Grocery grocery, ApiDbContext db) =>
{
    db.Groceries.Add(grocery);
    await db.SaveChangesAsync();
    return Results.Created($"/shoppingList/{grocery.Id}", grocery);
});

app.MapGet("/shoppingList/{id}", async (int id, ApiDbContext db) =>
{
    var grocery = await db.Groceries.FindAsync(id);

    return grocery != null? Results.Ok(grocery) : Results.NotFound();
});

app.MapDelete("/shoppingList/{id}", async (int id, ApiDbContext db) =>
{
    var grocery = await db.Groceries.FindAsync(id);
    if (grocery != null)
    {
        db.Groceries.Remove(grocery);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.MapPut("/shoppingList/{id}", async (int id, Grocery grocery, ApiDbContext db) =>
{
    var groceryInDb = await db.Groceries.FindAsync(id);
    if (groceryInDb != null)
    {
        groceryInDb.Name = grocery.Name;
        groceryInDb.Purchased = grocery.Purchased;
        var groceryUpdated = db.Groceries.Update(groceryInDb);
        await db.SaveChangesAsync();
        return Results.Ok(groceryUpdated.Entity);
    }
    return Results.NotFound();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();