using MyLittleContentEngine.Models;

namespace Stormancer.Docs;

public class BaseDocsFrontMatter : IFrontMatter
{
    public string Title { get; init; } = "Empty title";
    public string Description { get; init; } = string.Empty;
    public string? Uid { get; init; } = null;

    public DateTime Date { get; init; } = DateTime.Now;
    public bool IsDraft { get; init; } = false;
    public string[] Tags { get; init; } = [];
    public string? RedirectUrl { get; init; }
    public string? Section { get; init; }
    public int Order { get; init; } = int.MaxValue;
    
    public Metadata AsMetadata()
    {
        return new Metadata()
        {
            Title = Title,
            Description = Description,
            LastMod = Date,
            RssItem = true,
            Order = Order
        };
    }
}

public class ConstellationFrontMatter : BaseDocsFrontMatter
{
}

public class InspireFrontMatter : BaseDocsFrontMatter
{
}