namespace StreetTreeApi.Controllers

open System
open System.Text
open System.Data.SQLite
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open StreetTreeApi
open System.Text.Json
open Dapper

[<ApiController>]
[<Route("[controller]")>]
type TreeController (logger : ILogger<TreeController>) =
    inherit ControllerBase()
 
    let jsonOptions = JsonSerializerOptions(PropertyNamingPolicy = JsonNamingPolicy.CamelCase)

    let openConnection () =
        let connection = new SQLiteConnection(Loader.connectionString)
        connection.Open()
        connection

    let closeConnection (connection: SQLiteConnection) =
        connection.Close()

    let createCommand (sql: string) (connection: SQLiteConnection) =
        new SQLiteCommand(sql, connection)

    // GET /trees/{id}
    [<HttpGet("{id}", Name = "GetTree")>]
    member _.GetTreeById (id) =
        task {
            let connection = openConnection()
            let parameters = { tree_id = id }
            let query = "SELECT * FROM Trees WHERE tree_id = @tree_id"
            let tree = connection.QueryFirstOrDefault<Tree>(query, parameters)
            return JsonSerializer.Serialize(tree, jsonOptions)
        }

    [<HttpGet>]
    member _.GetTrees (
        [<FromQuery>] species: string option,
        [<FromQuery>] health: string option, 
        [<FromQuery>] status: string option,
        [<FromQuery>] borough: string option, 
        [<FromQuery>] zipcode: string option,
        [<FromQuery>] curbLocation: string option, 
        [<FromQuery>] sort: string option,
        [<FromQuery>] page: string option, 
        [<FromQuery>] perPage: string option) =

        task {
            // Initialize connection
            use connection = openConnection()

            // Start building the SQL query and parameters dynamically
            let baseQuery = StringBuilder("SELECT * FROM Trees WHERE 1=1")
            let parameters = DynamicParameters()

            // Add conditions based on provided query parameters
            let addCondition (query: StringBuilder, paramName: string, paramValue: string option) =
                match paramValue with
                | Some value ->
                    query.AppendFormat(" AND {0} = @{0}", paramName) |> ignore
                    parameters.Add(paramName, value)
                | None -> ()

            addCondition(baseQuery, "spc_latin", species |> Option.map string)
            addCondition(baseQuery, "health", health |> Option.map string)
            addCondition(baseQuery, "status", status |> Option.map string)
            addCondition(baseQuery, "boroname", borough |> Option.map string)
            addCondition(baseQuery, "zipcode", zipcode |> Option.map string)
            addCondition(baseQuery, "curb_loc", curbLocation |> Option.map string)

            // Handle sorting, pagination
            let sortColumn = if String.IsNullOrWhiteSpace(sort.Value) then "tree_id" else sort.Value
            baseQuery.AppendFormat(" ORDER BY {0}", sortColumn) |> ignore
            let pageSize = match perPage with Some value -> int value | None -> 10
            let pageIndex = match page with Some value -> int value | None -> 1
            let offset = (pageIndex - 1) * pageSize
            baseQuery.AppendFormat(" LIMIT @PageSize OFFSET @Offset") |> ignore
            parameters.Add("PageSize", pageSize)
            parameters.Add("Offset", offset)

            // Execute query
            let queryResult = connection.Query<Tree>(baseQuery.ToString(), parameters)

            // Serialize and return the result
            let serializedResult = JsonSerializer.Serialize(queryResult, jsonOptions)
            return serializedResult
        }

    [<HttpGet("health-summary")>]
    member _.GetHealthSummary ([<FromQuery>] groupBy: string option) =
        task {
            use connection = openConnection()

            let baseQuery = 
                match groupBy with
                | Some "boroname" -> "SELECT boroname, health, COUNT(*) AS count FROM Trees GROUP BY boroname, health"
                | Some "zipcode" -> "SELECT zipcode, health, COUNT(*) AS count FROM Trees GROUP BY zipcode, health"
                | _ -> "SELECT health, COUNT(*) AS count FROM Trees GROUP BY health"

            let result = connection.Query(baseQuery)
            return JsonSerializer.Serialize(result, jsonOptions)
        }

    [<HttpGet("species")>]
    member _.GetSpecies () =
        task {
            use connection = openConnection()

            let query = "SELECT DISTINCT spc_latin, spc_common FROM Trees"

            let result = connection.Query(query)
            return JsonSerializer.Serialize(result, jsonOptions)
        }
