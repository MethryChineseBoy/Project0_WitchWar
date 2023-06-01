class Enemy_02 : FatherEnemy
{
    void Awake()
    {
        enemyName = "e_02";
        spriteX = 0.6f;
        spriteY = 0.7f;
        hp = 80;
        deadScore = 500;
        InitEnemyOnce();
    }

    private void OnEnable()
    {
        InitEnemy();
    }
}
