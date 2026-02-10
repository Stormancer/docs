using Mdazor;
using MonorailCss.Parser.Custom;
using MonorailCss.Theme;
using MyLittleContentEngine;
using MyLittleContentEngine.MonorailCss;
using MyLittleContentEngine.UI.Components;
using Stormancer.Docs;
using Stormancer.Docs.Components;
using Stormancer.Docs.Components.Layouts;
using Stormancer.Docs.Components.Reference;
using Stormancer.Docs.Components.Shared;
using System.Collections.Immutable;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents();
builder.Services.AddContentEngineService(_ => new ContentEngineOptions 
{ 
    SiteTitle="Stormancer SDKs documentation",
    SiteDescription="Realtime, secure, distributed applications framework for C# and C++",
    ContentRootPath="Content"
})    // Console documentation service
    .WithMarkdownContentService(_ => new MarkdownContentOptions<ConstellationFrontMatter>
    {
        ContentPath = "Content/constellation",
        BasePageUrl = "/constellation",
        TableOfContentsSectionKey = "Constellation",

    })
    // CLI documentation service
    .WithMarkdownContentService(_ => new MarkdownContentOptions<InspireFrontMatter>
    {
        ContentPath = "Content/inspire",
        BasePageUrl = "/inspire",
        TableOfContentsSectionKey = "inspire",
    })
    // Blog service
    .WithMarkdownContentService(_ => new MarkdownContentOptions<BlogFrontMatter>
    {
        ContentPath = "Content/blog",
        BasePageUrl = "/blog",
        ExcludeSubfolders = false,
        PostFilePattern = "*.md;*.mdx"
    }).AddMdazor()
    .AddMdazorComponent<Step>()
    .AddMdazorComponent<Steps>()
    .AddMdazorComponent<Screenshot>()
    .AddMdazorComponent<WidgetApiReference>()
    .AddMdazorComponent<TwoColumn>()
    .AddMdazorComponent<Column>()
    .AddMonorailCss(_ => new MonorailCssOptions
    {
        ColorScheme = new AlgorithmicColorScheme()
        {
            PrimaryHue = 200,
            ColorSchemeGenerator = i => (i + 260, i + 15, i - 15),
            BaseColorName = ColorNames.Neutral,
        },
        CustomCssFrameworkSettings = settings =>
        {
            return settings = settings with
            {
                CustomUtilities = [
                new UtilityDefinition()
                {
                    Pattern = "scrollbar-thin",
                    Declarations = ImmutableList.Create(
                        new CssDeclaration("scrollbar-width", "thin")
                    )
                },
                new UtilityDefinition
                {
                    Pattern = "scrollbar-thumb-*",
                    IsWildcard = true,
                    Declarations = ImmutableList.Create(
                        new CssDeclaration("--tw-scrollbar-thumb-color", "--value(--color-*)")
                    )
                },
                new UtilityDefinition
                {
                    Pattern = "scrollbar-track-*",
                    IsWildcard = true,
                    Declarations = ImmutableList.Create(
                        new CssDeclaration("--tw-scrollbar-track-color", "--value(--color-*)")
                    )
                },
                new UtilityDefinition
                {
                    Pattern = "scrollbar-color",
                    Declarations = ImmutableList.Create(
                        new CssDeclaration("scrollbar-color", "var(--tw-scrollbar-thumb-color) var(--tw-scrollbar-track-color)")
                    )
                }
            ]
            };
        },
        // .net 10.0.101 has a bug flash grey on all content change and not removing it.
        // this hides empty error messages on hot reload
        ExtraStyles = """
                      #dotnet-compile-error:empty {
                          display: none;
                      }
                      """
    });
var app = builder.Build();

app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>();

// this adds the route for styles.css which is generated dynamically based on the used
// CSS classes.
app.UseMonorailCss();

await app.RunOrBuildContent(args);
