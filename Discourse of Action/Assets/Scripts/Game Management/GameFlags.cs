using System;

[Flags]
public enum GameFlags
{
    Undefined = 0,
    CanFightAster = 1,
    HasFoughtAster = 2,
    HasFoughtPrimrose = 3,
    HasFoughtIrisPhase1 = 4,
    HasFoughtIrisPhase2 = 5
}
