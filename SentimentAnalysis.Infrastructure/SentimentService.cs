// Import the necessary namespaces
using Microsoft.Extensions.Options;
using SentimentAnalysis.Core;// Required for PredictionEnginePool
using ModelTrainer.Models; // Required for ModelInput and ModelOutput   
using Microsoft.Extensions.ML; // Required for PredictionEnginePool

using System.IO; // Required for FileStream
using System.Threading.Tasks; // Required for Task

namespace SentimentAnalysis.Infrastructure;

// This class implements the contract defined in our Core project.
public class SentimentService : ISentimentService
{
    // A private field to hold the path to our model file.
    // ADD THIS LINE:
    private readonly PredictionEnginePool<ModelInput, ModelOutput> _predictionEnginePool;

    // This is CONSTRUCTOR INJECTION.
    // The DI container will see this and automatically pass in the IOptions<ModelOptions> object
    // that we configured in Program.cs on Day 1.
   // REPLACE THE OLD CONSTRUCTOR WITH THIS NEW ONE:
    public SentimentService(PredictionEnginePool<ModelInput, ModelOutput> predictionEnginePool)
    {
        _predictionEnginePool = predictionEnginePool;
    }
    
    // Add this method to SentimentService.cs
    // REPLACE THE OLD PredictAsync METHOD WITH THIS NEW ONE:
    public Task<SentimentPrediction> PredictAsync(SentimentRequest request, CancellationToken cancellationToken)
    {
        // Create a ModelInput instance from the request text.
        var modelInput = new ModelInput { Text = request.Text };

        // Use the pool to make a prediction.
        // We specify our model's unique name here.
        ModelOutput prediction = _predictionEnginePool.Predict(modelName: "SentimentAnalysisModel", example: modelInput);

        // Convert the boolean prediction (true/false) to a human-readable string.
        string sentiment = prediction.Prediction ? "Positive" : "Negative";

        // Create the final response object.
        var sentimentPrediction = new SentimentPrediction(sentiment);

        // Return the result, wrapped in a Task.
        return Task.FromResult(sentimentPrediction);
    }

}