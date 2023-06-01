namespace Assets.VirusKillerProject.scripts.Factorys.BullFactory
{
    class Bull_01 : FatherBullet
    {
        void Awake()
        {
            damage = 7;
            flySpeed = 7f;
            hitScore = 100;
            InitBulletOnce();
        }

        public override int GetDamage()
        {
            return damage;
        }

        public override int GetHitScore()
        {
            return hitScore;
        }

        void OnEnable()
        {
            InitBullet();
        }

        void Update()
        {
            ActiveInUpdate();
        }

        void FixedUpdate()
        {
            ActiveInFixedUpdate();
        }
    }
}
