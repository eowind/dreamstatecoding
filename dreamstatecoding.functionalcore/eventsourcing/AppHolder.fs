namespace dreamstatecoding.functionalcore


module AppHolder  =
    open dreamstatecoding.functionalcore.ApplicationState
    open System

    let mutable private state = AppliationState.Default
    let mutable private counter : int = 0
    let mutable private hasReplayed : bool = false;
    let mutable private replayCompleted : bool = false;
    let GetCurrentState() =  state
    let GetProcessedActionsCounter() = counter
    let IsFirstTimeUse() = not hasReplayed
    let IsReplayCompleted() = replayCompleted

    type Message =
        | Snapshot of AppliationState
        | Replay of obj
        | ReplayCompleted
        | Action of obj

    let private Processor = MailboxProcessor.Start(fun inbox ->
        let rec loop (s : AppliationState, c : int) =
            async {
                let! message = inbox.Receive()
                let c' = c + 1
                let s' =
                    match message with
                    | Snapshot snapshot -> snapshot
                    | Replay a -> s.HandleAction a
                    | ReplayCompleted ->
                        replayCompleted <- true
                        s
                    | Action a -> 
                        AppPersister.Persist a s c'
                        s.HandleAction a
                state <- s'
                counter <- c'
                return! loop (s', c')
                }
        loop (AppliationState.Default, 0))

    let HandleAction (action:obj) =
        Processor.Post (Action action)
             
             
    let private HandleReplayAction (action:obj, id:Guid) =
        hasReplayed <- true
        Processor.Post (Replay action)

    let InitiateFromLastSnapshot () =
        let (snapshot, actions) = AppPersister.GetLatestSnapshotAndActions()
        match snapshot with
        | Some x -> Processor.Post (Snapshot x)
        | _ -> ()
        Array.map (fun x -> (HandleReplayAction x)) actions |> ignore
        if hasReplayed then Processor.Post ReplayCompleted
        
    let InitiateFromActionsOnly () =
        AppPersister.GetAllActions()
        |> Array.map (fun x -> (HandleReplayAction x))
        |> ignore
        if hasReplayed then Processor.Post ReplayCompleted

