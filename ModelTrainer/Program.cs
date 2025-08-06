using Microsoft.ML;
using Microsoft.ML.Data;
using ModelTrainer.Models;

// --- Main Program ---
// Create a new ML.NET environment
var context = new MLContext();

// Create some dummy training data
var trainingData = new[]
{
    new ModelInput { Text = "This was a wonderful experience", Sentiment = true },
    new ModelInput { Text = "I am so happy", Sentiment = true },
    new ModelInput { Text = "This was horrible", Sentiment = false },
    new ModelInput { Text = "I'm so angry about this", Sentiment = false }
};

// Load the data into an IDataView
var dataView = context.Data.LoadFromEnumerable(trainingData);

// Build the training pipeline
var pipeline = context.Transforms.Text
    .FeaturizeText("Features", nameof(ModelInput.Text)) // Step 1: Convert text to numbers
    .Append(context.BinaryClassification.Trainers.SdcaLogisticRegression(
        labelColumnName: nameof(ModelInput.Sentiment),  // Step 2: Tell the trainer the label is in the "Sentiment" column
        featureColumnName: "Features"));                 // And the features are in the "Features" column
// Train the model
Console.WriteLine("Training model...");
var model = pipeline.Fit(dataView);
Console.WriteLine("Model trained.");

// Save the model to the location our API project expects it
Console.WriteLine("Saving model...");
// IMPORTANT: This path assumes you are running the command from the root folder of the solution
string modelPath = "../SentimentAnalysis.Api/ml_models/sentiment_model.zip";
context.Model.Save(model, dataView.Schema, modelPath);

Console.WriteLine($"Model saved to: {Path.GetFullPath(modelPath)}");