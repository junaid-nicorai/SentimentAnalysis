// Import all necessary namespaces
using SentimentAnalysis.Core;
using SentimentAnalysis.Infrastructure;
using Microsoft.Extensions.ML;
using ModelTrainer.Models;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURE SERVICES (The "DI Container") ---

// This registers all the classes needed for API Controllers to work.
builder.Services.AddControllers();

// This registers the services needed for Swagger UI.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// This binds our appsettings.json section to our ModelOptions class.
builder.Services.Configure<ModelOptions>(builder.Configuration.GetSection("ModelOptions"));

// Add this line
builder.Services.AddScoped<IPredictionService, PredictionServiceAdapter>();

// This registers our SentimentService as a singleton implementation for the ISentimentService interface.
// // ADD THESE LINES:
builder.Services.AddPredictionEnginePool<ModelInput, ModelOutput>()
    .FromFile(modelName: "SentimentAnalysisModel",
              filePath: builder.Configuration["ModelOptions:ModelPath"], 
              watchForChanges: true);

// ADD THIS LINE (it can go right after the pool registration):
builder.Services.AddScoped<ISentimentService, SentimentService>();
// --- 2. BUILD THE APPLICATION ---
var app = builder.Build();

// --- 3. CONFIGURE THE HTTP REQUEST PIPELINE (The "Middleware") ---

// This block enables Swagger UI *only* when we are in the "Development" environment.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// This line enables the routing system to send requests to our controllers.
// It is ABSOLUTELY ESSENTIAL. If this line is missing, you will get 404 errors.
app.MapControllers();

// --- 4. RUN THE APPLICATION ---
app.Run();


public partial class Program { }