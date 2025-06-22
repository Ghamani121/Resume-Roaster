using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class RoastRequest
    {
        [FromForm(Name = "File")]
        public IFormFile File { get; set; }
    }
}
