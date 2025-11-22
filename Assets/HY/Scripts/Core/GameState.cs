    public enum GameState { 
        Boot, 
        Playing, 
        Lobby,
        Paused, 
        LevelUpSelect, 
        GameOver, 
        Transition 
    }

/*
 /*
플레이어 : 이동, 공격, 회피, 레벨업
일반 적 :이동
보스 : 패턴, 특수 공격, 공격, 이동

단일화

public interface IDamge {
    void TakeDamage(int amount);
    void Die();
    int CurrentHp { get; }
}
 
씬 -> 적 

피격(인터페이스)

public class Player : Mono, IDamge{
    public int CureentHp {get; private set;}
    public void TakeDamage (int amount){
        CurrentHp -= amount;
    }

    public void Die(){
        게임오버 처리 -> 함수 끝
    }
}

public class Enemy : Mono, IDamge {
    public int CureentHp {get; private set;}
    public void TakeDamage (int amount){
        CurrentHp -= amount;
    }

    public void Die(){
        리스폰 메시지 or 사라지거나 or 사망 애니메이션 실행
    }
}

public class Boss : Mono, Idamge {
    public int CureentHp {get; private set;}

    public void TakeDamage (int amount){
        CurrentHp -= amount;

        if (CurrentHp < 50)
            Enter ();
    }

    public void Die(){
        리스폰 메시지 or 사라지거나 or 사망 애니메이션 실행
    }

    private void Enter2 () { 패턴 } 

=>플레이어, 적, 보스 코드는 서로 전혀 모르는데, 그냥 데미지 시스템이란 인터페이스만 보고 함수 충돌 X
----------------------------------------------------------------------------------------------------
공격! 

public interface Iattacker{
    void Attack();
    float AttackRange {get; }
}

public class Palyer : Mono, IAttacker, Idamge {
    public float AtRan => 2f;
    
    public void Attack(){
         콤보, 애니메이션, 판정 등
    }
}

public class Boss : Mono, IAttacker, Idamge {
    public float AtRan => 2f;
    
    public void Attack(){
         패턴
   }


AI 가 공격 가능한 객체를 다룰 때 객체 타입 신경 안써도 됨

public void TryAttack (IAttacker At){
    At.Attack();
}

인터페이스 객체
1. 공통 행동 규약
2. 다형성
    맞추는 대상이 누군지 모르지만, IDa 있으면 동작

3. 의존성 축소 -> 결합도 X
    Player, Enemy 클래스 몰라도 됨

4. 확장성 
새로운 Enemy, 새로운 Boss 그냥 인터페이스 구현 기존 시스템 그대로 이어짐


1. 데미지 시스템
2. 상태 시스템
3. 상호 작용 시스템
4. AI
5. 이벤트 처리
1st -> 재사용성 (2st)
 */



/*
인터페이스 : 규칙
피격
데미지
죽음
현재 체력

구조체 : 데이터 묶음

공격 정보 (데미지, 넉백 등)
이동 입력 (방향, 속도 등) 
상태 변화 (스턴 시간, 이펙트 타입 등)
피격 정보 (데미지, 히트 타입 등)

public interface Idamgeable{
    void TakeDamge(DamageInfo damageInfo);
}

public struct DamageInfo{
    public int Amount;
    pbblic Vector3 HitPoint;
    public float knockbackForce;
    pubilc DamageType Type;

    public DamageInfo(int amount, Vector3 hitPoint, float kan, Da type){
        Amount = amount;
        HitPoint = hit;
..
..
    }
}

public class Enemy : Mono, Idamgeable , ㅑㅇ마ㅣ어리{
    current HP = hp;

    public void TakeDamge(DamageInfo info){
    current HPhp -= info.Amount;
    ApplyK(info.HitPoint, info.Knock);
    PlayHitEffect(info.Type);
}

    Die()
-----
피격
현재체력
다이

}
----------------------------
DamageInfo info = new DamageInfo (
    amount = 20;
    hitPoint = transform.postionl
    kan = 2f;
    type : DamageType.2
)
target.TakeDame(info)



public struct StateContext {{
    public Transform Target;
    public float Distance;
    public bool IsInAttackRange;
}

public interface IState{
    void OnEnter ( StateContext ctx);
    void OnUpdate(StateContext ctx)
    void OExit ();
}
*/