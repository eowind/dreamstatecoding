namespace dreamstatecoding.functionalcore

open dreamstatecoding.functionalcore.Stores.UserStore

module ApplicationState =

    type AppliationState =
        {
            UserStore : UserStore
        }
        static member Default = 
            {
                UserStore = { Users = Map.empty }
            }
        member this.HandleAction (action:obj) =
            {
                UserStore = this.UserStore.HandleAction action
            }