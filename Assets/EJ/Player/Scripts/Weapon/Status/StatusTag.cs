/// 오브젝트가 현재 어떤 상태이상(Status)에 걸려있는지를 표현하는 비트 플래그.
/// - [Flags] 속성을 통해 여러 상태를 동시에 묶어서 저장할 수 있다.

using System;

[Flags]
public enum StatusTag
{
    none = 0,        
    shiledBreak = 1 << 0,  
    stun = 1 << 1, 
    nanoHeal = 1 << 2,   
    antiBoss       = 1 << 3,   
    antiDrone      = 1 << 4,   
    magnetic      = 1 << 5,  
}