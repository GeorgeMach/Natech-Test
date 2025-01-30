using AwesomeCatApi.Context;
using AwesomeCatApi.Models;
using Microsoft.EntityFrameworkCore;

public interface ICatService
{
    Task FetchCatsAsync();
    Task<CatEntity?> GetCatByIdAsync(int id);
    Task<(IEnumerable<CatEntity>, int)> GetCatsAsync(string? tag, int page, int pageSize);
}

public class CatService : ICatService
{
    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _catApiKey;

    public CatService(AppDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _catApiKey = configuration["ApiKeys:CatApiKey"];
    }

    public async Task FetchCatsAsync()
    {
        var httpClient = _httpClientFactory.CreateClient(nameof(CatService));
        httpClient.DefaultRequestHeaders.Add("x-api-key", _catApiKey);

        var response = await httpClient.GetFromJsonAsync<List<CatApiResponse>>("https://api.thecatapi.com/v1/images/search?limit=25&has_breeds=1");

        foreach (var apiCat in response)
        {
            // Check for duplicate cats
            if (await _context.Cats.AnyAsync(c => c.CatId == apiCat.Id))
                continue;

            // Process tags
            var tagNames = apiCat.Breeds
                .SelectMany(b => b.Temperament.Split(',')
                .Select(t => t.Trim()))
                .Distinct();

            var tags = new List<TagEntity>();
            foreach (var tagName in tagNames)
            {
                // Check if the tag already exists
                var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                if (existingTag != null)
                {
                    tags.Add(existingTag); // Reuse the existing tag
                }
                else
                {
                    var newTag = new TagEntity { Name = tagName };
                    tags.Add(newTag);
                    _context.Tags.Add(newTag); // Add new tag to the DbContext
                }
            }

            // Add the cat with its tags
            _context.Cats.Add(new CatEntity
            {
                CatId = apiCat.Id,
                Width = apiCat.Width,
                Height = apiCat.Height,
                ImageUrl = apiCat.Url,
                Image = await httpClient.GetByteArrayAsync(apiCat.Url),
                Tags = tags
            });
        }

        await _context.SaveChangesAsync();
    }

    public async Task<CatEntity?> GetCatByIdAsync(int id)
    {
        return await _context.Cats.Include(c => c.Tags).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<(IEnumerable<CatEntity>, int)> GetCatsAsync(string? tag, int page, int pageSize)
    {
        IQueryable<CatEntity> query = _context.Cats.Include(c => c.Tags);

        if (!string.IsNullOrEmpty(tag))
        {
            query = query.Where(c => c.Tags.Any(t => t.Name == tag));
        }

        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return (items, totalCount);
    }
}
