using Microsoft.Extensions.ML;
using ModelTrainer.Models;
using SentimentAnalysis.Core;

namespace SentimentAnalysis.Infrastructure;

public class PredictionServiceAdapter : IPredictionService
{
    private readonly PredictionEnginePool<ModelInput, ModelOutput> _pool;

    public PredictionServiceAdapter(PredictionEnginePool<ModelInput, ModelOutput> pool)
    {
        _pool = pool;
    }

    public ModelOutput Predict(ModelInput input)
    {
        return _pool.Predict(modelName: "SentimentAnalysisModel", example: input);
    }
}