using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private const int enemyMin = 1;
    private const int enemyMax = 1;

    [SerializeField]
    private GameObject EnemyObj;

    void Start()
    {
        //敵の数を抽選
        EnemyLottery();
        //特殊アビリティ効果発動
        //行動順番抽選(2ターン分)
    }

    private void EnemyLottery()
    {
        //敵の数を抽選
        int num = Random.Range(enemyMin, enemyMax + 1);

        for(int i = 0; i < num; i++)
        {
            //敵を生成
            if (EnemyObj != null) return;
            GameObject obj = Instantiate(EnemyObj);
            
        }
    }
}