using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class RoastRequest
    {
        [Required]
        [SwaggerSchema("Upload a resume in PDF or DOCX format")]
        public IFormFile File { get; set; }
    }
}
