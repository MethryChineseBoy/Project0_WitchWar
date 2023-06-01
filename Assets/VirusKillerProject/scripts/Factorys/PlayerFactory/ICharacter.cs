namespace Assets.VirusKillerProject.scripts.Factorys.PlayerFactory
{
    interface ICharacter
    {
        //将子弹伤害加到角色伤害上
        int AddBulletDamage(IBullet bullet);
        //取子弹发射间隔
        int GetShotIntervalTime();
    }
}
