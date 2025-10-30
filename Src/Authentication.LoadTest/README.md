# Authentication.LoadTest

A simple console application for load testing the AutoMapper configuration in the Authentication.Login project. This tool allows developers to:

- Test the behavior of `AuthenticationLoginProfileMapperInitializer.Mapper` under load
- Validate DebuggerDisplay attributes on mapped objects
- Test breakpoints and conditional debugging scenarios
- Reproduce and diagnose mapping issues

## Usage

### Basic Usage (Default Parameters)

```bash
dotnet run --project Src/Authentication.LoadTest/Authentication.LoadTest.csproj
```

Default parameters:
- **Iterations**: 10,000
- **Parallelism**: Environment.ProcessorCount (number of CPU cores)

### Custom Parameters

```bash
dotnet run --project Src/Authentication.LoadTest/Authentication.LoadTest.csproj -- --iterations 50000 --parallelism 8
```

### Parameters

- `--iterations <number>`: Number of mapping operations to perform (default: 10000)
- `--parallelism <number>`: Maximum degree of parallelism for concurrent operations (default: Environment.ProcessorCount)

## Examples

### Example 1: Low load test
```bash
dotnet run --project Src/Authentication.LoadTest/Authentication.LoadTest.csproj -- --iterations 1000 --parallelism 2
```

### Example 2: High load test
```bash
dotnet run --project Src/Authentication.LoadTest/Authentication.LoadTest.csproj -- --iterations 100000 --parallelism 16
```

### Example 3: Single-threaded test (for debugging)
```bash
dotnet run --project Src/Authentication.LoadTest/Authentication.LoadTest.csproj -- --iterations 100 --parallelism 1
```

## What It Tests

The load test creates an `AccountPayLoadDTO` instance with sample data and repeatedly maps it to an `Account` domain object using AutoMapper. This exercises:

1. The mapper initialization in `AuthenticationLoginProfileMapperInitializer`
2. The mapping profile defined in `AuthenticationLoginProfileMapping`
3. The DebuggerDisplay attributes on both DTO and domain objects
4. Thread-safety of the mapper under concurrent load

## Output

The program outputs:
- Configuration (iterations and parallelism)
- Sample DTO data
- Test results:
  - Total iterations
  - Successful mappings
  - Failed mappings
  - Total execution time
  - Throughput (operations per second)

## Debugging Tips

1. **Set breakpoints** in the Program.cs file to inspect mapped objects
2. **Use conditional breakpoints** on the iteration counter to stop at specific iterations
3. **Reduce parallelism to 1** for easier step-through debugging
4. **Check DebuggerDisplay** by hovering over mapped objects in the debugger
5. **Use low iteration counts** (e.g., 10-100) when actively debugging

## Building

The project is included in the main solution:

```bash
dotnet build Solution/Authentication.sln
```

Or build just the load test project:

```bash
dotnet build Src/Authentication.LoadTest/Authentication.LoadTest.csproj
```
