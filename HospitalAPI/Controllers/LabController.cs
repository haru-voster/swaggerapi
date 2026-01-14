using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HospitalAPI.Data;
using HospitalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabController : ControllerBase
    {
        private readonly DataApplicationDBContext _context;
        private readonly HttpClient _httpClient;
        private readonly ILogger<LabController> _logger;

        public LabController(DataApplicationDBContext context, ILogger<LabController> logger)
        {
            _context = context;
            _logger = logger;
            _httpClient = new HttpClient();
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddLabRequest([FromBody] LabRequest request)
        {
            if (request == null)
                return BadRequest("Request body cannot be null.");

            try
            {
                // Step 1: Insert into SQL database using EF Core
                await _context.LabRequests.AddAsync(request);
                await _context.SaveChangesAsync();

                // Step 2: Post same data to external LIMS API
                string apiUrl = "http://157.230.183.65:5050/v1/labRequest/post";
                string apiKey = "<YOUR_API_KEY>"; // replace with your actual key

                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return Ok(new
                    {
                        message = "Lab request saved successfully to SQL and posted to LIMS.",
                        lims_response = responseBody
                    });
                }
                else
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("LIMS API call failed: {Error}", errorBody);
                    return Ok(new
                    {
                        message = "Saved to SQL, but failed to send to LIMS.",
                        lims_error = errorBody
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding lab request.");
                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetLabRequests()
        {
            var labRequests = await _context.LabRequests.ToListAsync();
            return Ok(labRequests);
        }
    }
}
