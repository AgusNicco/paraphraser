class Program
{

    private class ParaphraseRequest
    {
        public string Text { get; set; }
    }

    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddCors();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var app = builder.Build();
        app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());


        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapGet("/", () => "We are ready!");

        app.MapPost("/paraphrase", async (ParaphraseRequest request) =>
        {
            if (string.IsNullOrWhiteSpace(request.Text))
            {
                return Results.BadRequest("Text is required.");
            }
            string paraphrasedText = await Logic.Paraphrase(request.Text);

            var output = new
            {
                Text = paraphrasedText
            };
            Console.WriteLine(output);

            return Results.Ok(output);
        });


        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

