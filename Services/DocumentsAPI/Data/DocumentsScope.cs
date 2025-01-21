using DocumentsAPI.Repositories;
using Shared.Services.App;

internal class DocumentsScope :Scope
{
    public override void CreateScope(IServiceCollection services)
    {
        services.AddScoped<DocumentTemplatesRepository>();
    }
}