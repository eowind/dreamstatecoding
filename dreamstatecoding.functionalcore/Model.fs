namespace hunting.core
open System;

module Model =
    type UserId = Guid
    type User = 
        {
            Id : UserId
            Name : string
            Email : string
        }

    type WeaponType = Unknown | Rifle | Shotgun
    type AmmunitionCaliberId = Guid
    type AmmunitionFireType = Unknown | Centerfire | Rimfire | Bore | Gauge | Air
    type AmmunitionCaliber = 
        {
            Id : AmmunitionCaliberId;
            FireType : AmmunitionFireType
            Name : string
        }
    type WeaponId = Guid
    type Weapon =
        {
            Id : WeaponId;
            Owner : UserId
            Name : string
            Type : WeaponType
            Brand : string
            Model : string
            AmmunitionCaliber : AmmunitionCaliberId
            AmmunitionCapacity : int;
            ShotsFired : int
        }

    type SeriesType = Unknown | Clays | Target | MovingGroundTarget
    type SeriesTemplate =
        {
            Type : SeriesType
            Shots : int
            Repititions : int
        }