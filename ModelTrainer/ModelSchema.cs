namespace ModelTrainer.Models; // Add this namespace

using Microsoft.ML.Data;

// Define the input data schema
public class ModelInput
{
    [LoadColumn(0)]
    public string Text { get; set; }= string.Empty; // Text input for sentiment analysis

    [LoadColumn(1)]
    public bool Sentiment { get; set; } // true for positive, false for negative
}

// Define the output prediction schema
public class ModelOutput
{
    [ColumnName("PredictedLabel")]
    public bool Prediction { get; set; }

    public float Score { get; set; }
}