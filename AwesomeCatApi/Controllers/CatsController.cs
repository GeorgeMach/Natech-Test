using AwesomeCatApi.Context;
using AwesomeCatApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AwesomeCatApi.Controllers
{
    [ApiController]
    [Route("api/cats")]
    public class CatsController : ControllerBase
    {
        private readonly ICatService _catService;

        public CatsController(ICatService catService)
        {
            _catService = catService;
        }
        /// <summary>
        /// Fetches new cats from The Cat API and stores them in the database
        /// </summary>
        [HttpPost("fetch")]
        public async Task<IActionResult> FetchCats()
        {
            await _catService.FetchCatsAsync();
            return Ok("Cats fetched successfully!");
        }

        /// <summary>
        /// Retrieves a specific cat by its database ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCat(int id)
        {
            var cat = await _catService.GetCatByIdAsync(id);
            if (cat == null) return NotFound("Cat not found");

            return Ok(cat);
        }
        /// <summary>
        /// Retrieves a paginated list of cats with optional tag filtering
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCats(
            [FromQuery] string? tag,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var (cats, totalCount) = await _catService.GetCatsAsync(tag, page, pageSize);

            return Ok(new { cats, totalCount });
        }
    }
}
