public class AuthenticationWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        // Adicione configurações extras de teste aqui se necessário
    }

    public void SeedTestData()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LoginContext>();

        context.Database.EnsureCreated();

        // Limpa os dados existentes para evitar duplicidade de chave
        context.dbAccountClaimAction.RemoveRange(context.dbAccountClaimAction.ToList());
        context.dbClaimAction.RemoveRange(context.dbClaimAction.ToList());
        context.dbAction.RemoveRange(context.dbAction.ToList());
        context.SaveChanges();

        // Adiciona dados de teste sem duplicidade de chave
        context.dbAction.Add(new Authentication.Login.Domain.Implementation.Action
        {
            Id = 1,
            Name = "Test Action"
        });
        context.dbAction.Add(new Authentication.Login.Domain.Implementation.Action
        {
            Id = 2,
            Name = "Test Action 2"
        });

        context.dbClaimAction.Add(new ClaimAction
        {
            Id = 1,
            IdClaim = 1,
            IdAction = 1
        });
        context.dbClaimAction.Add(new ClaimAction
        {
            Id = 2,
            IdClaim = 2,
            IdAction = 2
        });

        context.dbAccountClaimAction.Add(new AccountClaimAction
        {
            Id = 1,
            IdAccount = 1,
            IdClaimAction = 1
        });
        context.dbAccountClaimAction.Add(new AccountClaimAction
        {
            Id = 2,
            IdAccount = 2,
            IdClaimAction = 2
        });

        context.SaveChanges();
    }
}
