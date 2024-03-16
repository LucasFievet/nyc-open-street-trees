module HealthSummaryTests

open Xunit
open Xunit.Abstractions
open StreetTreeApi.Controllers
open System.Text.Json

let jsonOptions = JsonSerializerOptions(PropertyNamingPolicy = JsonNamingPolicy.CamelCase)

// Helper to deserialize JSON to a specific type
let deserialize<'T> (json: string) : 'T =
    JsonSerializer.Deserialize<'T>(json, jsonOptions)


type HealthCount = {
   Zipcode: int64
   Boroname: string
   Health: string
   Count: int
}


type HealthSummaryTests (output: ITestOutputHelper) =

    // Initializes the controller with a test logger
    let controller = TreeController(null)

    // Test without grouping
    [<Fact>]
    member _.``Get Health Summary without Grouping`` () =
        let task = controller.GetHealthSummary(None)
        let result = task.Result

        // Deserialize the result to check the content
        let summary = deserialize<seq<HealthCount>> (result)

        // Assertions
        Assert.NotNull(result)
        output.WriteLine(sprintf "Result: %s" result)

        Assert.True(summary |> Seq.exists (fun item -> item.Health = "Good" && item.Count > 0))
        Assert.True(summary |> Seq.exists (fun item -> item.Health = "Fair" && item.Count > 0))
        Assert.True(summary |> Seq.exists (fun item -> item.Health = "Poor" && item.Count > 0))

    // Test grouping by borough
    [<Fact>]
    member _.``Get Health Summary Grouped by Borough`` () =
        let task = controller.GetHealthSummary(Some "boroname")
        task.Wait()
        let result = task.Result

        // Deserialize the result to check the content
        let summary = deserialize<seq<HealthCount> >(result)

        // Assertions
        Assert.NotNull(result)
        output.WriteLine(sprintf "Result: %s" result)

        // Replace "SomeBorough" with actual borough names from your test data
        Assert.True(summary |> Seq.exists (fun item -> item.Boroname = "Bronx" && item.Health = "Good" && item.Count > 0))

    // Test grouping by zipcode
    [<Fact>]
    member _.``Get Health Summary Grouped by Zipcode`` () =
        let task = controller.GetHealthSummary(Some "zipcode")
        task.Wait()
        let result = task.Result

        // Deserialize the result to check the content
        let summary = deserialize<seq<HealthCount> >(result)

        // Assertions
        Assert.NotNull(result)
        output.WriteLine(sprintf "Result: %s" result)

        // Replace "SomeZipcode" with actual zip code values from your test data
        Assert.True(summary |> Seq.exists (fun item -> item.Zipcode = 10474 && item.Health = "Good" && item.Count > 0))
