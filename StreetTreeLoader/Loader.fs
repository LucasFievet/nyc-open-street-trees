module Loader

open System
open System.Data.SQLite
open System.IO
open CsvHelper
open System.Globalization

// Define a connection string to the SQLite database.
let connectionString = $"Data Source=street-trees.db; Version=3;"

// Define a function to create a SQLite connection.
let createConnection() = new SQLiteConnection(connectionString)

// Define the SQL statement to create the table if it does not exist.
let createTableSql = """
CREATE TABLE IF NOT EXISTS Trees (
    created_at TEXT,
    tree_id INTEGER PRIMARY KEY,
    block_id INTEGER,
    the_geom TEXT,
    tree_dbh INTEGER,
    stump_diam INTEGER,
    curb_loc TEXT,
    status TEXT,
    health TEXT,
    spc_latin TEXT,
    spc_common TEXT,
    steward TEXT,
    guards TEXT,
    sidewalk TEXT,
    user_type TEXT,
    problems TEXT,
    root_stone TEXT,
    root_grate TEXT,
    root_other TEXT,
    trnk_wire TEXT,
    trnk_light TEXT,
    trnk_other TEXT,
    brnch_ligh TEXT,
    brnch_shoe TEXT,
    brnch_othe TEXT,
    address TEXT,
    zipcode INTEGER,
    zip_city TEXT,
    cb_num INTEGER,
    borocode INTEGER,
    boroname TEXT,
    cncldist INTEGER,
    st_assem INTEGER,
    st_senate INTEGER,
    nta TEXT,
    nta_name TEXT,
    boro_ct INTEGER,
    state TEXT,
    Latitude REAL,
    longitude REAL,
    x_sp REAL,
    y_sp REAL
);
"""

// Define a function to create the table.
let createTable (conn: SQLiteConnection) =
    use cmd = new SQLiteCommand(createTableSql, conn)
    conn.Open()
    cmd.ExecuteNonQuery() |> ignore
    conn.Close()

// Define a function to read from the CSV and insert into the SQLite database.
let loadCsvIntoDatabase (csvFilePath: string) (conn: SQLiteConnection) =
    conn.Open()
    
    // Start a transaction.
    use transaction = conn.BeginTransaction()

    use reader = new StreamReader(csvFilePath)
    use csv = new CsvReader(reader, CultureInfo.InvariantCulture)

    // Read the header of the CSV file.
    if csv.Read() = false then failwith "The CSV file is empty or incorrectly formatted."
    csv.ReadHeader() |> ignore
    let headers = csv.HeaderRecord
    if headers = null then failwith "Headers could not be read."

    // Prepare the INSERT statement.
    let insertSql = sprintf "INSERT INTO Trees (%s) VALUES (%s);" 
                            (String.Join(",", headers))
                            (String.Join(",", headers |> Array.map (fun _ -> "?")))
    use cmd = new SQLiteCommand(insertSql, conn)
    headers |> Array.iter (fun header -> cmd.Parameters.AddWithValue(header, null) |> ignore)

    // Read the CSV file and insert each row into the SQLite database.
    while csv.Read() do
        headers |> Array.iteri (fun index header ->
            let value = csv.GetField(header)
            cmd.Parameters.[index].Value <- value
        )
        
        // Print information here. Adjust the field name as appropriate for your CSV structure.
        let treeId = csv.GetField("tree_id")
        printfn "Inserting tree with ID: %s" treeId

        cmd.ExecuteNonQuery() |> ignore

    transaction.Commit()
    conn.Close()

// Define the main function to execute the above-defined functions.
[<EntryPoint>]
let main argv =
    // TODO: automate downloading the CSV file
    let csvFilePath = "2015StreetTreesCensus_TREES.csv"
    use conn = createConnection()
    createTable conn
    loadCsvIntoDatabase csvFilePath conn
    0 // Return 0 to indicate success
