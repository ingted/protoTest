module vpon.protobuf

open Google.Protobuf
open System.IO
open System
open Newtonsoft.Json
open gRTB
open Protobuf.Comm.Doubleclick
open Newtonsoft.Json.Linq


let bArr2Json (bArr:byte[]) =
    try
        let br = new BidRequest()
        br.MergeFrom(bArr) //read message from protodat file
        let formatter = 
            new JsonFormatter(
                new JsonFormatter.Settings(false)
                )
        let jsonString = formatter.Format(br)
        let parsedJson = JToken.Parse(jsonString)
        let beautified = parsedJson.ToString(Formatting.Indented)
        beautified
    with
    | exn ->
        exn.Message

let bin2json p =
    try
        let input = File.OpenRead(p) 
        let bArr = Array.zeroCreate (int input.Length)
        ignore <| input.Read(bArr, 0, int input.Length)
        bArr2Json bArr
    with
    | exn ->
        exn.Message


let brArr2Json (bArr:byte[]) =
    try
        let br = new BidResponse()
        br.MergeFrom(bArr) //read message from protodat file
        let formatter = 
            new JsonFormatter(
                new JsonFormatter.Settings(false)
                )
        let jsonString = formatter.Format(br)
        let parsedJson = JToken.Parse(jsonString)
        let beautified = parsedJson.ToString(Formatting.Indented)
        beautified
    with
    | exn ->
        exn.Message



let json2bin jsonString p =
    try 
        let brObj = JsonConvert.DeserializeObject<BidRequest>(jsonString, new ProtoMessageConverter())
        let fi = new FileInfo(p)
        if fi.Exists then fi.Delete()
        let fs = new FileStream(p, FileMode.CreateNew)
        brObj.WriteTo fs
        fs.Flush()
        fs.Close()
        fs.Dispose()
    with
    | exn ->
        printfn "%s" exn.Message





[<EntryPoint>]
let main argv =
    //let input = File.OpenRead(@"Z:\froto\proto\noPmpEncode") 
    //let bArr = Array.zeroCreate (int input.Length)
    //ignore <| input.Read(bArr, 0, int input.Length)
    //let br = new BidRequest()
    //br.MergeFrom(bArr) //read message from protodat file
    //let formatter = 
    //    new JsonFormatter(
    //        new JsonFormatter.Settings(false)
    //        )
    //let jsonString = formatter.Format(br)
    let jsonString = bin2json @"Z:\froto\proto\noPmpEncode"
    System.IO.File.WriteAllText(@"./br.json", jsonString)
    printfn "%s" jsonString

    let brObj = JsonConvert.DeserializeObject<BidRequest>(jsonString, new ProtoMessageConverter())
    //let js = JsonSerializer.Create()
    //let jr = JsonReader.
    //let brObj = js.Deserialize<BidRequest>(jsonString)
    printfn "%A" brObj


    json2bin jsonString @".\protoBin"

    let jsonString2 = bin2json @".\protoBin"
    System.IO.File.WriteAllText(@"./br2.json", jsonString2)


    let jStr = System.IO.File.ReadAllText @"./brPMP.json"
    json2bin jStr @".\protoBinPmp"

    System.Console.ReadLine() |> ignore
    0 // return an integer exit code
    