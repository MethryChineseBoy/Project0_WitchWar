using UnityEngine;

namespace Assets.VirusKillerProject.scripts.Factorys.PlayerFactory
{
    public class Character_02:MonoBehaviour,ICharacter
    {
        private int _baseDamage = 55;
        private int _shotIntervalTime = 20;

        public int AddBulletDamage(IBullet bullet)
        {
            return _baseDamage + bullet.GetDamage();
        }

        public int GetShotIntervalTime()
        {
            return _shotIntervalTime;
        }
    }
}
