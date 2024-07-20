using System;

[Flags] public enum GameFlags
{
    Undefined = 0,
    CanFightAster = 1 << 0,
    HasFoughtAster = 1 << 1,
    CanFightPrimrose = 1 << 2,
    HasFoughtPrimrose = 1 << 3,
    CanFightIris = 1 << 4,
    HasFoughtIrisPhase1 = 1 << 5,
    HasFoughtIrisPhase2 = 1 << 6
}
