using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using grp_management.Data;
using grp_management.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace grp_management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class TemplatesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TemplatesController> _logger;

        public TemplatesController(AppDbContext context, ILogger<TemplatesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTemplate([FromBody] Template template)
        {
            try
            {
                // Extract placeholders from TemplateMsg and add to Placeholders dictionary
                var extractedPlaceholders = new Dictionary<string, string>();
                var matches = Regex.Matches(template.TemplateMsg, @"{{\s*([a-zA-Z0-9_]+)\s*}}");
                foreach (Match match in matches)
                {
                    var key = match.Groups[1].Value;
                    if (!extractedPlaceholders.ContainsKey(key))
                    {
                        extractedPlaceholders[key] = ""; // Default empty value
                    }
                }

                // Merge with existing placeholders (if any) or initialize
                Dictionary<string, string> existingPlaceholders = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(template.Placeholders))
                {
                    try
                    {
                        existingPlaceholders = JsonSerializer.Deserialize<Dictionary<string, string>>(template.Placeholders) ?? new Dictionary<string, string>();
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogWarning(ex, "Failed to deserialize existing placeholders for template ID {TemplateID}. Resetting.", template.TemplateID);
                        existingPlaceholders = new Dictionary<string, string>();
                    }
                }

                foreach (var kvp in extractedPlaceholders)
                {
                    if (!existingPlaceholders.ContainsKey(kvp.Key))
                    {
                        existingPlaceholders[kvp.Key] = kvp.Value;
                    }
                }
                template.Placeholders = JsonSerializer.Serialize(existingPlaceholders);


                if (template.TemplateID > 0)
                {
                    // Update existing template
                    var existingTemplate = await _context.Templates.FindAsync(template.TemplateID);
                    if (existingTemplate == null)
                    {
                        return NotFound(new { message = "Template not found" });
                    }

                    existingTemplate.TemplateName = template.TemplateName;
                    existingTemplate.TemplateMsg = template.TemplateMsg;
                    existingTemplate.TemplateType = template.TemplateType;
                    existingTemplate.Placeholders = template.Placeholders; // Use the updated placeholders string

                    _context.Templates.Update(existingTemplate);
                }
                else
                {
                    // Create new template
                    _context.Templates.Add(template);
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "Template saved successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving template");
                return StatusCode(500, new { message = "An error occurred while saving the template" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemplate(int id)
        {
            try
            {
                var template = await _context.Templates.FindAsync(id);
                if (template == null)
                {
                    return NotFound(new { message = "Template not found" });
                }

                _context.Templates.Remove(template);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Template deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting template");
                return StatusCode(500, new { message = "An error occurred while deleting the template" });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetTemplates()
        {
            try
            {
                var templates = await _context.Templates.ToListAsync();
                return Ok(templates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving templates");
                return StatusCode(500, new { message = "An error occurred while retrieving templates" });
            }
        }
    }
} 