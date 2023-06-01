namespace Assets.VirusKillerProject.scripts.Factorys.BullFactory
{
    class Bull_02 : FatherBullet
    {
        void Awake()
        {
            damage = 70;
            flySpeed = 5f;
            hitScore = 300;
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
