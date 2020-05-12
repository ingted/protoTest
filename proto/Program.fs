// Learn more about F# at https://fsharp.org
// See the 'F# Tutorial' project for more help.

open Google.Protobuf
open System.IO

[<EntryPoint>]
let main argv =
    let input = File.OpenRead("") 
    
    let br = new BidRequest()
    br.MergeFrom(input) //read message from protodat file
    let formatter = 
        new JsonFormatter(
            new JsonFormatter.Settings(false)
            )
    let jsonString = formatter.Format(br)
    System.IO.File.WriteAllText("", jsonString)
    
    0 // return an integer exit code
