public interface IBullet
{
    //获取子弹伤害
    int GetDamage();
    //子弹击中敌人后加分回收
    void HitBullet();
    //获取子弹得分
    int GetHitScore();
    //获取子弹的当前生命
    int GetBulletHp();
    //获取子弹的ID
    int GetId();
    //设置子弹的ID
    void SetId(int value);
}
