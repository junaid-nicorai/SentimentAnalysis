// Make sure all these 'using' statements are at the top.
using Microsoft.AspNetCore.Mvc;
using SentimentAnalysis.Core;
using System.Threading;
using System.Threading.Tasks;

namespace SentimentAnalysis.Api.Controllers;

// CHECK 1: Do you have these two attributes?
[ApiController]
[Route("api/[controller]")] 
public class SentimentController : ControllerBase
{
    private readonly ISentimentService _sentimentService;

    public SentimentController(ISentimentService sentimentService)
    {
        _sentimentService = sentimentService;
    }

    // CHECK 2: Is the [HttpPost] attribute directly above the method?
    [HttpPost]
    public async Task<IActionResult> Predict([FromBody] SentimentRequest request, CancellationToken cancellationToken)
    {
        var prediction = await _sentimentService.PredictAsync(request, cancellationToken);

        return Ok(prediction);
    }
}