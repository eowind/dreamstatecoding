namespace dreamstatecoding.functionalcore
open System;

module Actions =
    open Model

    type AddUser = { ActionId: Guid; User : User }
    type ModifyUser = { ActionId: Guid; User : User }
    

