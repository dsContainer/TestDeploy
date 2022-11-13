using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Infrastructure.Model.TemplateModel
{
    public class TemplateModel
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? File { get; set; }

    }
}
