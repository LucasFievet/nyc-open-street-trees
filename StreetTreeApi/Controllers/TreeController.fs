namespace StreetTreeApi.Controllers

open System.Data.SQLite
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open StreetTreeApi

[<ApiController>]
[<Route("[controller]")>]
type TreeController (logger : ILogger<TreeController>) =
    inherit ControllerBase()
 
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
            let cmd = createCommand (sprintf "SELECT * FROM Trees WHERE tree_id = %i" id) connection
            use reader = cmd.ExecuteReader()
            let data = reader.Read()
            closeConnection connection
            return data
            //return! json data next ctx
        }
