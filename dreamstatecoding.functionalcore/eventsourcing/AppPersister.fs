namespace dreamstatecoding.functionalcore


module AppPersister =
    open System
    open System.Reflection
    open System.IO
    open Newtonsoft.Json
    open FSharp.Interop.Dynamic
 
    
    let private store = @".\store\"

    
    let private createSnapshotFilename (nextActionId:Guid) =
        let now = DateTime.UtcNow
        let dayPath = now.ToString("yyyy-MM-dd")
        let fullPath = Path.Combine(store, "snapshots", dayPath)
        let di = new DirectoryInfo(fullPath)
        di.Create()
        let filename = now.ToString("yyyy-MM-dd hh_mm_ss_fffffff.") + nextActionId.ToString() + ".snapshot"
        let fullFilename = Path.Combine(fullPath, filename)
        fullFilename
    
    let private writeSnapshotToDisk  (state:obj) (nextActionId:Guid) =
        let fullFilename = createSnapshotFilename nextActionId
        let json = JsonConvert.SerializeObject(state)
        File.WriteAllText(fullFilename,  json)
        ()

    let private Snapshotter = MailboxProcessor.Start(fun inbox ->
        let rec loop (n:int) =
            async {
                let! (actionCounter:int,state:obj, nextActionId:Guid) = inbox.Receive()
                if actionCounter % 300 = 0 then
                    writeSnapshotToDisk state nextActionId
                return! loop n
                }
        loop 0)

    let PersistSnapshot (actionCounter:int) (state:obj) (nextActionId:Guid) =
        Snapshotter.Post (actionCounter, state, nextActionId)



    let private createActionFilename (action:obj) (actionId:Guid) =
        let now = DateTime.UtcNow
        let hourPath = now.ToString("yyyy-MM-dd HH")
        let fullPath = Path.Combine(store, hourPath)
        let di = new DirectoryInfo(fullPath)
        di.Create()
        let t = action.GetType()
        let filename = now.ToString("yyyy-MM-dd hh_mm_ss_fffffff+") + now.Ticks.ToString() + "." + t.Name + "." + actionId.ToString() +  ".action"
        let fullFilename = Path.Combine(fullPath, filename)
        fullFilename

    let PersistAction (action:obj) (actionId:Guid) =
        let fullFilename = createActionFilename action actionId
        let json = JsonConvert.SerializeObject(action)
        File.WriteAllText(fullFilename,  json)
        ()

    let Persist (nextAction:obj) (state:obj) (actionCounter:int)  =
        PersistAction nextAction nextAction?ActionId
        PersistSnapshot actionCounter state nextAction?ActionId
        ()

    let private getAction (json:string, filename:string) =
        let split = filename.Split('.')
        let actionName = split.[1]
        let actionNameWithNamespace = "dreamstatecoding.functionalcore.Actions+" + actionName
        let t = Assembly.GetExecutingAssembly().GetType(actionNameWithNamespace)
        (JsonConvert.DeserializeObject(json, t), Guid.Parse(split.[2]))

    let GetAllActions () =
        let di = new DirectoryInfo(store)
        let actions =
            di.GetFiles("*.action", SearchOption.AllDirectories)
            |> Seq.map (fun (fi:FileInfo) -> File.ReadAllText(fi.FullName), fi.Name)
            |> Seq.map (fun x -> (getAction x))
            |> Seq.toArray
        actions
    

    let private fileContainAction (filename:string) (actionIdString:string) =
        let split = filename.Split('.')
        split.[2] = actionIdString

    let GetAllActionsFromId (nextActionId:Guid) =
        let di = new DirectoryInfo(store)
        di.Create()
        let nextActionIdString = nextActionId.ToString()
        let actions =
            di.GetFiles("*.action", SearchOption.AllDirectories)
            |> Seq.skipWhile (fun (fi:FileInfo) -> not (fileContainAction fi.Name nextActionIdString))
            |> Seq.map (fun (fi:FileInfo) -> File.ReadAllText(fi.FullName), fi.Name)
            |> Seq.map (fun x -> (getAction x))
            |> Seq.toArray
        actions
        
    let private getSnapshot (fi:FileInfo) =
        let split = fi.Name.Split('.')
        let json = File.ReadAllText(fi.FullName)
        (JsonConvert.DeserializeObject<ApplicationState.AppliationState>(json), Guid.Parse(split.[1]))

    let GetLatestSnapshotAndActions () =
        let di = new DirectoryInfo(Path.Combine(store, "snapshots"))
        di.Create()
        let fileInfos = di.GetFiles("*.snapshot", SearchOption.AllDirectories)
        match fileInfos with
        | a when a.Length = 0 -> (None, GetAllActions())
        | _ ->
            let (snapshot, nextActionId) =
                fileInfos
                |> Seq.last
                |> getSnapshot
            let actions = GetAllActionsFromId nextActionId
            (Some snapshot, actions)
        