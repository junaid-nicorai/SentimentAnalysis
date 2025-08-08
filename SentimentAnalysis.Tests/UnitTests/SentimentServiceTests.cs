using ModelTrainer.Models;
using Moq;
using SentimentAnalysis.Core;
using SentimentAnalysis.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SentimentAnalysis.Tests.UnitTests;

public class SentimentServiceTests
{
    private readonly Mock<IPredictionService> _mockPredictionService; // Mock the INTERFACE
    private readonly SentimentService _sentimentService;

    public SentimentServiceTests()
    {
        // ARRANGE
        _mockPredictionService = new Mock<IPredictionService>(); // Mock the INTERFACE
        _sentimentService = new SentimentService(_mockPredictionService.Object); // Inject the MOCK
    }

    [Fact]
    public async Task PredictAsync_WhenPredictionIsTrue_ShouldReturnPositive()
    {
        // ARRANGE
        var request = new SentimentRequest("This is a great test");
        var fakeModelOutput = new ModelOutput { Prediction = true };

        // Setup the mock: When the Predict method is called, return our fake output.
        _mockPredictionService
            .Setup(p => p.Predict(It.IsAny<ModelInput>())) // Setup the INTERFACE method
            .Returns(fakeModelOutput);

        // ACT
        var result = await _sentimentService.PredictAsync(request, CancellationToken.None);

        // ASSERT
        Assert.Equal("Positive", result.Sentiment);
    }
}