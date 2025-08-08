using Microsoft.AspNetCore.Mvc.Testing; // The WebApplicationFactory
using SentimentAnalysis.Core;
using System.Net.Http.Json; // For PostAsJsonAsync and ReadFromJsonAsync
using System.Threading.Tasks;
using Xunit;

namespace SentimentAnalysis.Tests.IntegrationTests;

// This class uses IClassFixture to create a single instance of WebApplicationFactory
// that will be shared across all tests in this class. This is efficient.
public class SentimentApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SentimentApiTests(WebApplicationFactory<Program> factory)
    {
        // The factory creates an in-memory test server and a client to talk to it.
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_Predict_WithPositiveText_ReturnsPositiveSentiment()
    {
        // ARRANGE
        // 1. Define the request body we will send.
        var request = new SentimentRequest("This was a wonderful experience, I am so happy.");

        // ACT
        // 2. Send a REAL HTTP POST request to our in-memory API.
        //    PostAsJsonAsync handles serializing our request object to JSON.
        var httpResponse = await _client.PostAsJsonAsync("/api/sentiment", request);

        // ASSERT
        // 3. Assert that the HTTP response was successful (e.g., status code 200 OK).
        httpResponse.EnsureSuccessStatusCode();

        // 4. Deserialize the JSON response body back into our prediction object.
        var sentimentPrediction = await httpResponse.Content.ReadFromJsonAsync<SentimentPrediction>();

        // 5. Assert that the prediction is not null and has the correct sentiment.
        Assert.NotNull(sentimentPrediction);
        Assert.Equal("Positive", sentimentPrediction.Sentiment);
    }

    [Fact]
    public async Task Post_Predict_WithNegativeText_ReturnsNegativeSentiment()
    {
        // ARRANGE
        var request = new SentimentRequest("This was horrible, I am angry.");

        // ACT
        var httpResponse = await _client.PostAsJsonAsync("/api/sentiment", request);

        // ASSERT
        httpResponse.EnsureSuccessStatusCode();
        var sentimentPrediction = await httpResponse.Content.ReadFromJsonAsync<SentimentPrediction>();
        Assert.NotNull(sentimentPrediction);
        Assert.Equal("Negative", sentimentPrediction.Sentiment);
    }
}