using Authentication.Login.DTO;
using Authentication.Login.Mapping;

namespace Authentication.LoadTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Parse command-line arguments
            int iterations = 10000;
            int parallelism = Environment.ProcessorCount;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--iterations" && i + 1 < args.Length)
                {
                    if (int.TryParse(args[i + 1], out int parsedIterations))
                    {
                        iterations = parsedIterations;
                    }
                }
                else if (args[i] == "--parallelism" && i + 1 < args.Length)
                {
                    if (int.TryParse(args[i + 1], out int parsedParallelism))
                    {
                        parallelism = parsedParallelism;
                    }
                }
            }

            Console.WriteLine($"Authentication Load Test");
            Console.WriteLine($"Iterations: {iterations}");
            Console.WriteLine($"Parallelism: {parallelism}");
            Console.WriteLine();

            // Create a sample Authentication DTO with real values
            var authDto = new AccountPayLoadDTO
            {
                Id = 1,
                UserName = "testuser@example.com",
                Password = "SecureP@ssw0rd123!",
                CreatedBy = "LoadTestSystem",
                UpdatedBy = "LoadTestSystem"
            };

            Console.WriteLine($"Sample DTO - UserName: {authDto.UserName}, Id: {authDto.Id}");
            Console.WriteLine("Starting load test...");

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Execute mapping iterations in parallel
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = parallelism
            };

            long successCount = 0;
            long errorCount = 0;

            Parallel.For(0, iterations, options, i =>
            {
                try
                {
                    // Map AccountPayLoadDTO to Account
                    var account = AuthenticationLoginProfileMapperInitializer.Mapper.Map<Authentication.Login.Domain.Implementation.Account>(authDto);

                    // Verify the mapping worked
                    if (account != null && account.UserName == authDto.UserName)
                    {
                        Interlocked.Increment(ref successCount);
                    }
                    else
                    {
                        Interlocked.Increment(ref errorCount);
                    }
                }
                catch (Exception ex)
                {
                    Interlocked.Increment(ref errorCount);
                    Console.WriteLine($"Error in iteration {i}: {ex.Message}");
                }
            });

            stopwatch.Stop();

            Console.WriteLine();
            Console.WriteLine("Load test completed!");
            Console.WriteLine($"Total iterations: {iterations}");
            Console.WriteLine($"Successful mappings: {successCount}");
            Console.WriteLine($"Failed mappings: {errorCount}");
            Console.WriteLine($"Total time: {stopwatch.Elapsed.TotalSeconds:F2} seconds");
            Console.WriteLine($"Throughput: {iterations / stopwatch.Elapsed.TotalSeconds:F2} operations/second");
        }
    }
}
