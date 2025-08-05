// Import the necessary namespaces
using Microsoft.Extensions.Options;
using SentimentAnalysis.Core;
using System.IO; // Required for FileStream
using System.Threading.Tasks; // Required for Task

namespace SentimentAnalysis.Infrastructure;

// This class implements the contract defined in our Core project.
public class SentimentService : ISentimentService
{
    // A private field to hold the path to our model file.
    private readonly string _modelPath;

    // This is CONSTRUCTOR INJECTION.
    // The DI container will see this and automatically pass in the IOptions<ModelOptions> object
    // that we configured in Program.cs on Day 1.
    public SentimentService(IOptions<ModelOptions> options)
    {
        // We access the ".Value" property to get the actual ModelOptions object.
        _modelPath = options.Value.ModelPath;
    }
    
    // Add this method to SentimentService.cs
    public Task<SentimentPrediction> PredictAsync(SentimentRequest request, CancellationToken cancellationToken)
    {
        // On Day 4, this is where we will use the ML model.
        // For today, we return a hardcoded result to prove the pipeline works.
        var dummyPrediction = new SentimentPrediction("Positive");
        
        return Task.FromResult(dummyPrediction);
    }

    // This is a placeholder method to demonstrate async file loading.
    // In a real application, this might be part of the service's initialization.
    public async Task LoadModelAsync()
    {
        // This 'await using' statement is the key to safe async resource management.
        // It creates a FileStream to our model file.
        // When the block is exited, it automatically and asynchronously calls DisposeAsync() on the stream,
        // which closes the file handle correctly, even if an error occurs.
        await using var fileStream = new FileStream(_modelPath, FileMode.Open, FileAccess.Read);

        // We are not actually loading an ML.NET model yet.
        // This just proves we can access the file asynchronously based on our configuration.
        // We use Task.Delay to simulate real async work.
        await Task.Delay(100);
    }
}