namespace StreetTreeApi

#nowarn "20"

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.OpenApi.Models

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =
        
        (*
            TODOs:
            - Setup dependency injection for logging, database connection, and other services
            - Repository pattern for data access
            - Load configuration from appsettings.json or environment variables
            - Setup monitoring and health checks
            - Add authentication and authorization logic
            - Background services for data processing and other tasks
            - Event queue to perform long running tasks
            - Staging tables for pre-computed data transformations
            - Use async/await for I/O bound operations

            - Prettier and linting as pre-commit hook
            - Properly comment and organize code
            - CI/CD pipeline to build, test, and deploy the application
        *)

        let builder = WebApplication.CreateBuilder(args)
        let services = builder.Services

        services.AddControllers()
        
        let info = OpenApiInfo()
        info.Title <- "My API V1"
        info.Version <- "v1"
        services.AddSwaggerGen(fun config -> config.SwaggerDoc("v1", info)) |> ignore

        let app = builder.Build()

        app.UseHttpsRedirection()

        app.UseAuthorization()
        app.MapControllers()

        app.UseSwagger() |> ignore
        app.UseSwaggerUI(fun config -> config.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenStreet API")) |> ignore

        app.Run()

        exitCode
