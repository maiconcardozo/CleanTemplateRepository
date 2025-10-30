namespace Authentication.API
{
    /// <summary>
    /// Classe principal da aplicação responsável pela inicialização e execução do host.
    /// Simplificada para usar o padrão Startup.cs para organização das configurações.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Método principal de entrada da aplicação.
        /// Cria e executa o host web usando as configurações definidas na classe Startup.
        /// </summary>
        /// <param name="args">Argumentos da linha de comando.</param>
        public static void Main(string[] args)
        {
            // Cria o builder da aplicação web com os argumentos fornecidos
            var builder = WebApplication.CreateBuilder(args);

            // Configura os serviços da aplicação usando a classe Startup
            // Isso promove uma melhor organização e separação de responsabilidades
            var startup = new Startup(builder.Configuration, builder.Environment);
            startup.ConfigureServices(builder.Services);

            // Constrói a aplicação com todas as configurações de serviços aplicadas
            var app = builder.Build();

            // Configura o pipeline de middlewares usando a classe Startup
            // Define como as requisições HTTP serão processadas
            startup.Configure(app, builder.Environment);

            // Inicia a execução da aplicação
            // A aplicação ficará em execução até receber um sinal de parada
            app.Run();
        }
    }
}

// Required for WebApplicationFactory in integration tests:
public partial class Program
{
}
