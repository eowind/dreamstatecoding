namespace dreamstatecoding.functionalcore
open System;

module Actions =
    open Model

    type AddUser = { ActionId: Guid; UserId : UserId; Name:string; Email:string }
    type ModifyUser = { ActionId: Guid; UserId : UserId; Name:string; Email:string }
    

