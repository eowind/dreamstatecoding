namespace hunting.core

open hunting.core.Stores.AmmunitionStore
open hunting.core.Stores.UserStore
open hunting.core.Stores.WeaponStore

module ApplicationState =

    type AppliationState =
        {
            UserStore : UserStore
            AmmunitionStore : AmmunitionStore
            WeaponStore : WeaponStore
        }
        static member Default = 
            {
                UserStore = { Users = Map.empty }
                AmmunitionStore = { Calibers = Map.empty }
                WeaponStore = { Weapons = Map.empty }
            }
        member this.HandleAction (action:obj) =
            {
                UserStore = this.UserStore.HandleAction action
                AmmunitionStore = this.AmmunitionStore.HandleAction action 
                WeaponStore = this.WeaponStore.HandleAction action 
            }