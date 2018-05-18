namespace dreamstatecoding.functionalcore
open System;

module Model =
    type UserId = Guid
    type User = 
        {
            Id : UserId
            Name : string
            Email : string
        }

