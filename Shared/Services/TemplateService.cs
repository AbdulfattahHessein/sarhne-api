using System.Reflection;
using Shared.Services.Interfaces;

namespace Shared.Services;

public class TemplateService : ITemplateService
{
    private readonly string _templatesPath;

    public TemplateService()
    {
        var projectPath =
            (Directory.GetParent(Directory.GetCurrentDirectory())?.FullName)
            ?? throw new InvalidOperationException("Path is the root directory");

        string templateProject =
            Assembly.GetExecutingAssembly().GetName().Name
            ?? throw new InvalidOperationException("Assembly name is null");

        _templatesPath = Path.Combine(projectPath, templateProject, "Templates");

        if (!Directory.Exists(_templatesPath))
            throw new DirectoryNotFoundException("Templates directory is not found");
    }

    public async Task<string> GetTemplateAsync(string templateName)
    {
        using var reader = new StreamReader(Path.Combine(_templatesPath, templateName));

        return await reader.ReadToEndAsync();
    }

    public string ReplaceInTemplate(string input, IDictionary<string, string> replaceWords)
    {
        var response = input;

        foreach (var temp in replaceWords)
            response = response.Replace(temp.Key, temp.Value);

        return response;
    }
}
