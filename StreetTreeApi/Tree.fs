namespace StreetTreeApi

open System

type Tree =
    { Date: DateTime
      TemperatureC: int
      Summary: string }

    member this.TemperatureF =
        32.0 + (float this.TemperatureC / 0.5556)
