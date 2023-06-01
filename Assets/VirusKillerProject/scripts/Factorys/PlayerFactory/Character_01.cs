using UnityEngine;

namespace Assets.VirusKillerProject.scripts.Factorys.PlayerFactory
{
    public class Character_01 : MonoBehaviour,ICharacter
    {
        private int _baseDamage = 1;
        private int _shotIntervalTime = 10;

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
