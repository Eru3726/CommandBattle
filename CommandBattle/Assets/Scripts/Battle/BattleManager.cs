using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    private const int enemyMin = 1;
    private const int enemyMax = 4;

    [SerializeField]
    private GameObject EnemyObj;

    private readonly float[,] enemyPos = 
        new float[,] {{   0f,    0f,     0f,    0f},
            　        { 2.1f, -2.1f,     0f,    0f},
                      { 3.5f,     0,  -3.5f,    0f},
                      {   5f, 1.65f, -1.65f,   -5f}};

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

        for (int i = 0; i < num; i++)
        {
            //敵を生成
            if (EnemyObj == null) return;
            GameObject obj = Instantiate(EnemyObj);

            //敵のポジション調整
            obj.transform.position = new Vector3(enemyPos[num - 1, i], 0, 0);
            obj.name = "Enemy" + (i + 1).ToString();
        }
    }
}