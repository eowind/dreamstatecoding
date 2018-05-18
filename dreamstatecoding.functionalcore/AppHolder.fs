namespace hunting.core


module AppHolder  =
    open hunting.core.ApplicationState
    open System

    let mutable private state = AppliationState.Default
    let mutable private counter : int = 0
    let GetCurrentState() =  state
    let GetProcessedActionsCounter() = counter

    type Message =
        | Snapshot of AppliationState
        | Replay of obj
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
        Processor.Post (Replay action)

    let InitiateFromLastSnapshot () =
        let (snapshot, actions) = AppPersister.GetLatestSnapshotAndActions()
        match snapshot with
        | Some x -> Processor.Post (Snapshot x)
        | _ -> ()
        Array.map (fun x -> (HandleReplayAction x)) actions
        
    let InitiateFromActionsOnly () =
        AppPersister.GetAllActions()
        |> Array.map (fun x -> (HandleReplayAction x))
        |> ignore
        ()

