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

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoastController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> RoastResume([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { error = "No file uploaded." });

            string extractedText = await ExtractText(file);

            if (string.IsNullOrWhiteSpace(extractedText))
                return BadRequest(new { error = "Could not extract text from the file." });

            string roast = await GetRoastFromOpenAI(extractedText);

            return Ok(new { roast });
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

        private async Task<string> GetRoastFromOpenAI(string resumeText)
        {
            string prompt = $"Roast this resume like a sarcastic stand-up comic. Be brutally honest but funny:\n\n{resumeText}";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YOUR_OPENAI_API_KEY");

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var response = await client.PostAsync("https://api.openai.com/v1/chat/completions",
                new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                return "Error: OpenAI API call failed.";

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
