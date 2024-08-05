using System;

[Flags] public enum GameFlags
{
    Undefined = 0,
    CanFightAster = 1 << 0,
    CanFightPrimrose = 1 << 1,
    CanFightIris = 1 << 2
}
