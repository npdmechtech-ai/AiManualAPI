using AiManual.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔥 REGISTER SERVICES
builder.Services.AddSingleton<SearchService>();
builder.Services.AddSingleton<OpenAIService>();
builder.Services.AddSingleton<ChatService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();