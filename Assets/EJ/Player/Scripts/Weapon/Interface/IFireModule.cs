/// 모든 무기의 공격 형태를 결정짓는 인터페이스이다.
/// 

using UnityEngine;

// 공격 형태 인터페이스
public interface IFireModule
{
    void Init(FireInitContext ctx);     // 런타임 무기/풀/오너 1회 주입
    void Fire(in FireContext ctx);      // 한 틱(트리거 1번) 발사
}

// 무기 초기화에 필요한 환경
public struct FireInitContext
{
    public RuntimeWeapon runtime;  // 메인코어 적용까지 끝난 값(프리팹/StatusTag 포함)
    public Transform owner;
    public IPool pool;
    // 서브코어 주입 
    public ISubCoreModule subCore;
}
// 발사체 Fire에 필요한 환경
public struct FireContext
{
    public Transform muzzle;
    public Vector2 aimDir;
    public int seed;         // 확산 랜덤 일관성
}