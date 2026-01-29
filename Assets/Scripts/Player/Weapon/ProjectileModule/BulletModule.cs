using UnityEngine;

namespace Attack.Pooling
{
    public sealed class BulletFireModule : IFireModule
    {
        private RuntimeWeapon _runtimeWeapon;      // 런타임 무기 데이터 (메인코어 반영 후)
        private Transform owner;      // 발사자 (플레이어)
        private IPool pool;           // 탄환 풀 매니저
        private ISubCoreModule sub;   // 서브코어 (가속 or 확산)

        // 무기 초기화 (무기 장착 시 1회 실행)
        public void Init(FireInitContext ctx)
        {
            _runtimeWeapon = ctx.runtime;
            owner = ctx.owner;
            pool = ctx.pool;
            sub = ctx.subCore;

            // 가속 코어가 있으면 fireRate 수정 (공속 증가)
            sub?.ApplyAccel(ref _runtimeWeapon);
        }

        // 발사 (매번 공격 트리거 시 실행)
        public void Fire(in FireContext ctx)
        {
            // 1️. 기본 발사 계획: 1발, AimDir 방향
            var basePlan = new ShotPlan
            {
                count = 1,
                dir = new[] { ctx.aimDir.normalized }
            };

            // 2. 서브코어(확산)가 있다면: 여러 발 + 각도 조정
            var plan = sub?.ApplyMultiple(_runtimeWeapon, ctx, basePlan) ?? basePlan;

            // 3. 최종 데미지 계산
            float finalDamage = _runtimeWeapon.RUN_damage;
            if (owner.TryGetComponent<PlayerStatus>(out var status))
            {
                finalDamage = status.GetFinalDamage(finalDamage);
            }

            // 4. 샷 수만큼 탄환 생성
            for (int i = 0; i < plan.count; i++)
            {   
                var s = new ShotStats
                {
                    dir = plan.dir[i],
                    speed = _runtimeWeapon.RUN_projectileSpeed,
                    lifetime = _runtimeWeapon.RUN_projectileLifetime
                };

                // 탄환 생성 및 초기화
                var go = pool.Get(_runtimeWeapon.bulletPrefabKey, ctx.muzzle.position, Quaternion.identity);
                var bullet = go.GetComponent<Bullet>();
                        
                bullet.Init(new Bullet.Args
                {
                    owner = owner,
                    dir = s.dir,
                    speed = s.speed,
                    life = s.lifetime,
                    damage = finalDamage,    
                    status = _runtimeWeapon.RUN_OnHitTags      
                });
            }
        }
    }
}
