module Tests

open Xunit
open Xunit.Abstractions
open StreetTreeApi.Controllers

type ``OpenStreetTreeController`` (output: ITestOutputHelper) =
    // Store the output helper in a field
    let output = output

    // Test method
    [<Fact>]
    member _.GetTreeByIdTest () =
        // Arrange
        let controller = TreeController(null) // Replace with appropriate constructor

        // Act
        output.WriteLine(sprintf "Result: %A" Loader.connectionString)
        let result = controller.GetTreeById(11).Result

        // Assert
        Assert.NotNull(result)
        output.WriteLine(sprintf "Result: %A" result)
        // Deserialize the result and assert on the properties

    // Example test for GetTrees
    [<Fact>]
    member _.GetTreesTest() =
        // Arrange
        let controller = TreeController(null)
        let expectedSpecies = Some "Acer rubrum"
        let expectedHealth = Some "Good"
        let expectedStatus = Some "Alive"
        let expectedBorough = None
        let expectedZipcode = None
        let expectedCurbLocation = None
        let expectedSort = Some "tree_id"
        let expectedPage = Some "1"
        let expectedPerPage = Some "10"

        // Act
        let task = controller.GetTrees(expectedSpecies, expectedHealth, expectedStatus, 
                                       expectedBorough, expectedZipcode, expectedCurbLocation,
                                       expectedSort, expectedPage, expectedPerPage)

        let result = task.Result

        // Assert
        Assert.NotNull(result)
        output.WriteLine(sprintf "Result: %s" result)
        // Deserialize the result and assert on the properties
