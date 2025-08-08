using ModelTrainer.Models; // You will need to add a reference from Core to ModelTrainer for this

namespace SentimentAnalysis.Core;

public interface IPredictionService
{
    ModelOutput Predict(ModelInput input);
}