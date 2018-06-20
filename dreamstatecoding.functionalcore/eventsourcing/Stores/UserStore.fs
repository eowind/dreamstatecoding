namespace dreamstatecoding.functionalcore.Stores

module UserStore =
    open System;
    open dreamstatecoding.functionalcore.Model
    open dreamstatecoding.functionalcore.Actions
    
    type UserStore =
        {
            Users: Map<UserId, User>
        }
        member this.AddUser (action:AddUser)=
            let user = 
                {
                    Id = action.UserId
                    Name = action.Name
                    Email = action.Name
                }
            { this with Users = this.Users.Add(user.Id, user); }

        member this.ModifyUser (action:ModifyUser)=
            let o = this.Users.TryFind action.UserId
            match o with
            | None -> this
            | Some original ->
                let updated = { original with Name = action.Name; Email = action.Email }
                { this with Users = this.Users.Add(updated.Id, updated); }
            
        member this.HandleAction (action:obj) =
            match action with
            | :? AddUser as a -> this.AddUser a
            | :? ModifyUser as a -> this.ModifyUser a
            | _ -> this
             

    