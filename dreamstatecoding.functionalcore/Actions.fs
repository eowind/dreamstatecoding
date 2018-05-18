namespace hunting.core
open System;

module Actions =
    open Model

    type AddUser = { ActionId: Guid; User : User }
    type ModifyUser = { ActionId: Guid; User : User }
    
    type AddAmmunitionCaliber = { ActionId: Guid; AmmunitionType : AmmunitionCaliber }
    type ModifyAmmunitionCaliber = { ActionId: Guid; AmmunitionType : AmmunitionCaliber }
    
    type AddWeapon = { ActionId: Guid; Weapon : Weapon }
    type ModifyWeapon = { ActionId: Guid; Weapon : Weapon }

