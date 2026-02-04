/// 오브젝트가 현재 어떤 상태이상(Status)에 걸려있는지를 표현하는 비트 플래그.
/// - [Flags] 속성을 통해 여러 상태를 동시에 묶어서 저장할 수 있다.

using System;

[Flags]
public enum StatusTag
{
    none = 0,        
    shiledBreak = 1 << 0,  // 쉴드 파괴
    stun = 1 << 1, // 스턴
    nanoHeal = 1 << 2, // 나노
    antiBoss       = 1 << 3,  // 안티 보스 
    antiDrone      = 1 << 4,   // 안티 드론
    knockBack      = 1 << 5,  // 넉백
}

/// 상태이상 적용법
/// 1. 위 StatusTag 열거형은 HitContext에 넣어보낼거임
/// 2. IDamageable를 상속받는 클래스에서 ApplyHit 부분에서 takeDamage 외에 ApplyStatus 같은 메서드를 둔다.
/// 3. void ApplyStatus(in HitContext ctx)
// {
//     if (ctx.status.HasFlag(StatusTag.ShieldBonus))
//         ApplyShieldBonus(ctx);

//     if (ctx.status.HasFlag(StatusTag.AntiDrone))
//         ApplyAntiDrone(ctx);
//     ~ 이하 생략 ~
// }
/// -> 다음과 같이 상태이상 받았을때 호출할 메서드를 바인딩한다.