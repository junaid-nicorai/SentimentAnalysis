
using System.Threading; // Required for CancellationToken
using System.Threading.Tasks; // Required for Task


namespace SentimentAnalysis.Core;

public interface ISentimentService
{
    // This interface is a contract.
    // We will define the specific methods it needs on Day 3.
    // For today, leaving it empty is perfect.

    Task<SentimentPrediction> PredictAsync(SentimentRequest request, CancellationToken cancellationToken);
}