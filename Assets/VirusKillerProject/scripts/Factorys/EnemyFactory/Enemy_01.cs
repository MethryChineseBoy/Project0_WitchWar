class Enemy_01 : FatherEnemy
{
    void Awake()
    {
        enemyName = "e_01";
        spriteX = 0.42f;
        spriteY = 0.42f;
        hp = 40;
        deadScore = 300;
        InitEnemyOnce();
    }

    private void OnEnable()
    {
        InitEnemy();
    }
}