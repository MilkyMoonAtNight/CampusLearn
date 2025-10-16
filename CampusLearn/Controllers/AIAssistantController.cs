using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CampusLearn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIAssistantController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public AIAssistantController(IConfiguration config)
        {
            _config = config;
            _httpClient = new HttpClient();
        }

        
        [HttpPost("ask")]
        public async Task<IActionResult> AskGemini([FromBody] AiQuestion request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Question))
                return BadRequest(new { error = "Question cannot be empty." });

           
            var apiKey = _config["Gemini:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
                return StatusCode(500, new { error = "Gemini API key not configured." });

            try
            {
                
                var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

                
                var payload = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = request.Question }
                            }
                        }
                    }
                };

                
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, new
                    {
                        error = $"Gemini API error: {response.StatusCode}",
                        details = responseString
                    });
                }

                using var doc = JsonDocument.Parse(responseString);
                var root = doc.RootElement;

                
                var aiText = root
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                

                return Ok(new
                {
                    question = request.Question,
                    answer = aiText
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An error occurred while communicating with Gemini.",
                    details = ex.Message
                });
            }
        }
    }

    // === Simple DTO for incoming question ===
    public class AiQuestion
    {
        public string? Question { get; set; }
    }
}