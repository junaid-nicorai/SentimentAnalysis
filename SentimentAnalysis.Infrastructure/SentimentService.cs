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
    private readonly IPredictionService _predictionService; // Change this line

    public SentimentService(IPredictionService predictionService) // Change this line
    {
        _predictionService = predictionService; // Change this line
    }

    public Task<SentimentPrediction> PredictAsync(SentimentRequest request, CancellationToken cancellationToken)
    {
        var modelInput = new ModelInput { Text = request.Text };

        // Use our clean interface here
        ModelOutput prediction = _predictionService.Predict(modelInput); // Change this line

        string sentiment = prediction.Prediction ? "Positive" : "Negative";
        var sentimentPrediction = new SentimentPrediction(sentiment);
        return Task.FromResult(sentimentPrediction);
    }
}