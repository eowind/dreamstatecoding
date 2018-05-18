namespace hunting.core.Stores

module UserStore =
    open System;
    open hunting.core.Model
    open hunting.core.Actions
    
    type UserStore =
        {
            Users: Map<UserId, User>
        }
        member this.AddUser (action:AddUser)=
            { this with Users = this.Users.Add(action.User.Id, action.User); }

        member this.ModifyUser (action:ModifyUser)=
            { this with Users = this.Users.Add(action.User.Id, action.User); }
            
        member this.HandleAction (action:obj) =
            match action with
            | :? AddUser as a -> this.AddUser a
            | :? ModifyUser as a -> this.ModifyUser a
            | _ -> this
             

    