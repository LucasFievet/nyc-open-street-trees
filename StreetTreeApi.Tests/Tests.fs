module Tests

open System
open Xunit
open Xunit.Abstractions // Make sure to open this namespace
open StreetTreeApi.Controllers

type ``OpenStreetTreeController_Get_Tree`` (output: ITestOutputHelper) =
    // Store the output helper in a field
    let output = output

    // Test method
    [<Fact>]
    member _.OpenStreetTreeController_Get_Tree () =
        // Arrange
        let controller = TreeController(null) // Replace with appropriate constructor

        // Act
        output.WriteLine(sprintf "Result: %A" Loader.connectionString)
        let result = controller.GetTreeById(11).Result

        // Assert
        Assert.NotNull(result)
        output.WriteLine(sprintf "Result: %A" result)
        //Assert.NotEmpty(result)
        //Assert.Equal(5, result.Length)
