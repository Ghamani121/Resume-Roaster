using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using DocumentFormat.OpenXml.Packaging; // For DOCX
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;          // For PDF
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using backend.Models;


namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoastController : ControllerBase
    {

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Roast controller is alive");
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> RoastResume([FromForm] RoastRequest request)
        {
            try
            {
                var file = request.File;

                if (file == null || file.Length == 0)
                    return BadRequest(new { error = "No file uploaded." });

                string extractedText = await ExtractText(file);

                if (string.IsNullOrWhiteSpace(extractedText))
                    return BadRequest(new { error = "Could not extract text from the file." });

                string roast = await GetRoastFromGroq(extractedText);

                return Ok(new { roast });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Exception occurred",
                    message = ex.Message,
                    stack = ex.StackTrace
                });
            }
        }


        private async Task<string> ExtractText(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLower();
            using var stream = file.OpenReadStream();

            if (ext == ".pdf")
            {
                var sb = new StringBuilder();
                using (var pdf = PdfDocument.Open(stream))
                {
                    foreach (var page in pdf.GetPages())
                    {
                        sb.AppendLine(page.Text);
                    }
                }
                return sb.ToString();
            }
            else if (ext == ".docx")
            {
                using var doc = WordprocessingDocument.Open(stream, false);
                return doc.MainDocumentPart.Document.Body.InnerText;
            }
            else
            {
                return null;
            }
        }

        private async Task<string> GetRoastFromGroq(string resumeText)
        {
            string prompt = $"Roast this resume like a sarcastic stand-up comic. Be brutally honest but funny:\n\n{resumeText}";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "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");

            var requestBody = new
            {
                model = "llama3-70b-8192",

                messages = new[]
                {
            new { role = "user", content = prompt }
        }
            };

            var response = await client.PostAsync("https://api.groq.com/openai/v1/chat/completions",
                new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                return $"Groq Error: {response.StatusCode} - {errorText}";
            }

            using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);

            return doc.RootElement
                      .GetProperty("choices")[0]
                      .GetProperty("message")
                      .GetProperty("content")
                      .GetString();
        }

    }
}
