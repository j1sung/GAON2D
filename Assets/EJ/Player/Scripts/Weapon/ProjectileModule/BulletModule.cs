using UnityEngine;

namespace Attack.Pooling
{
    public sealed class BulletFireModule : IFireModule
    {
        private RuntimeWeapon r;      // 런타임 무기 데이터 (메인코어 반영 후)
        private Transform owner;      // 발사자 (플레이어)
        private IPool pool;           // 탄환 풀 매니저
        private ISubCoreModule sub;   // 서브코어 (가속 or 확산)

        // 무기 초기화 (무기 장착 시 1회 실행)
        public void Init(FireInitContext ctx)
        {
            r = ctx.runtime;
            owner = ctx.owner;
            pool = ctx.pool;
            sub = ctx.subCore;

            // 가속 코어가 있으면 fireRate 수정 (공속 증가)
            sub?.ApplyAccel(ref r);
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
            var plan = sub?.ApplyMultiple(r, ctx, basePlan) ?? basePlan;

            // 3. 샷 수만큼 탄환 생성
            for (int i = 0; i < plan.count; i++)
            {
                var s = new ShotStats
                {
                    dir = plan.dir[i],
                    speed = r.RUN_projectileSpeed,
                    lifetime = r.RUN_projectileLifetime
                };

                // 탄환 생성 및 초기화
                var go = pool.Get(r.bulletPrefabKey, ctx.muzzle.position, Quaternion.identity);

                var bullet = go.GetComponent<Bullet>();
                bullet.Init(new Bullet.Args
                {
                    owner = owner,
                    dir = s.dir,
                    speed = s.speed,
                    life = s.lifetime,
                    damage = r.RUN_damage,    
                    status = r.RUN_OnHitTags      
                });
            }
        }
    }
}
