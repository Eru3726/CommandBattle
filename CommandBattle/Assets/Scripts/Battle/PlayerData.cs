using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class PlayerData : MonoBehaviour
{
    public int hp;
    public int mp;
    public int attackPower;
    public int magicPower;
    public int attackDefense;
    public int magicDefense;
    public int speed;

    private void Awake()
    {
        Load();
    }

    private void OnDestroy()
    {
        Save();
    }

    // 上書き情報の保存
    public void Save()
    {
#if UNITY_EDITOR
        //UnityEditor上なら
        //Assetファイルの中のSaveファイルのパスを入れる
        string path = Application.dataPath + "/Save";

#else
        //そうでなければ
        //.exeがあるところにSaveファイルを作成しそこのパスを入れる
        Directory.CreateDirectory("Save");
        string path = Directory.GetCurrentDirectory() + "/Save";

#endif

        //セーブファイルのパスを設定
        string SaveFilePath = path + "/PlayerData.bytes";

        // セーブデータの作成
        PlayerSaveData saveData = CreateSaveData();

        // セーブデータをJSON形式の文字列に変換
        string jsonString = JsonUtility.ToJson(saveData);

        // 文字列をbyte配列に変換
        byte[] bytes = Encoding.UTF8.GetBytes(jsonString);

        // AES暗号化
        byte[] arrEncrypted = AesEncrypt(bytes);

        // 指定したパスにファイルを作成
        FileStream file = new FileStream(SaveFilePath, FileMode.Create, FileAccess.Write);

        //ファイルに保存する
        try
        {
            // ファイルに保存
            file.Write(arrEncrypted, 0, arrEncrypted.Length);
        }
        finally
        {
            // ファイルを閉じる
            if (file != null)
            {
                file.Close();
            }
        }
    }

    public void Load()
    {
#if UNITY_EDITOR
        //UnityEditor上なら
        //Assetファイルの中のSaveファイルのパスを入れる
        string path = Application.dataPath + "/Save";

#else
        //そうでなければ
        //.exeがあるところにSaveファイルを作成しそこのパスを入れる
        Directory.CreateDirectory("Save");
        string path = Directory.GetCurrentDirectory() + "/Save";

#endif

        //セーブファイルのパスを設定
        string SaveFilePath = path + "/PlayerData.bytes";

        //セーブファイルがあるか
        if (File.Exists(SaveFilePath))
        {
            //ファイルモードをオープンにする
            FileStream file = new FileStream(SaveFilePath, FileMode.Open, FileAccess.Read);
            try
            {
                // ファイル読み込み
                byte[] arrRead = File.ReadAllBytes(SaveFilePath);

                // 復号化
                byte[] arrDecrypt = AesDecrypt(arrRead);

                // byte配列を文字列に変換
                string decryptStr = Encoding.UTF8.GetString(arrDecrypt);

                // JSON形式の文字列をセーブデータのクラスに変換
                PlayerSaveData saveData = JsonUtility.FromJson<PlayerSaveData>(decryptStr);

                //データの反映
                ReadData(saveData);

            }
            finally
            {
                // ファイルを閉じる
                if (file != null)
                {
                    file.Close();
                }
            }
        }
        else
        {
            //初期化
            hp = 50;
            mp = 20;
            attackPower = 5;
            magicPower = 5;
            attackDefense = 5;
            magicDefense = 5;
            speed = 5;
        }
    }

    // セーブデータの作成
    private PlayerSaveData CreateSaveData()
    {
        //セーブデータのインスタンス化
        PlayerSaveData saveData = new PlayerSaveData
        {
            hp = hp,
            mp = mp,
            attackPower = attackPower,
            magicPower = magicPower,
            attackDefense = attackDefense,
            magicDefense = magicDefense,
            speed = speed
        };

        return saveData;
    }

    //データの読み込み（反映）
    private void ReadData(PlayerSaveData saveData)
    {
        hp = saveData.hp;
        mp = saveData.mp;
        attackPower = saveData.attackPower;
        magicPower = saveData.magicPower;
        attackDefense = saveData.attackDefense;
        magicDefense = saveData.magicDefense;
        speed = saveData.speed;
    }

    /// <summary>
    ///  AesManagedマネージャーを取得
    /// </summary>
    /// <returns></returns>
    private AesManaged GetAesManager()
    {
        //任意の半角英数16文字
        string aesIv = "dsi9ug8473newu08";
        string aesKey = "dngw07423nuas8t4";

        AesManaged aes = new AesManaged
        {
            KeySize = 128,
            BlockSize = 128,
            Mode = CipherMode.CBC,
            IV = Encoding.UTF8.GetBytes(aesIv),
            Key = Encoding.UTF8.GetBytes(aesKey),
            Padding = PaddingMode.PKCS7
        };
        return aes;
    }

    /// <summary>
    /// AES暗号化
    /// </summary>
    /// <param name="byteText"></param>
    /// <returns></returns>
    public byte[] AesEncrypt(byte[] byteText)
    {
        // AESマネージャーの取得
        AesManaged aes = GetAesManager();
        // 暗号化
        byte[] encryptText = aes.CreateEncryptor().TransformFinalBlock(byteText, 0, byteText.Length);

        return encryptText;
    }

    /// <summary>
    /// AES復号化
    /// </summary>
    /// <param name="byteText"></param>
    /// <returns></returns>
    public byte[] AesDecrypt(byte[] byteText)
    {
        // AESマネージャー取得
        var aes = GetAesManager();
        // 復号化
        byte[] decryptText = aes.CreateDecryptor().TransformFinalBlock(byteText, 0, byteText.Length);

        return decryptText;
    }

    //セーブデータ削除
    public void Init()
    {
#if UNITY_EDITOR
        //UnityEditor上なら
        //Assetファイルの中のSaveファイルのパスを入れる
        string path = Application.dataPath + "/Save";

#else
        //そうでなければ
        //.exeがあるところにSaveファイルを作成しそこのパスを入れる
        Directory.CreateDirectory("Save");
        string path = Directory.GetCurrentDirectory() + "/Save";

#endif

        //ファイル削除
        File.Delete(path + "/PlayerData.bytes");

        //リロード
        Load();

        Debug.Log("データの初期化が終わりました");
    }
}


[Serializable]
public class PlayerSaveData
{
    public int hp;
    public int mp;
    public int attackPower;
    public int magicPower;
    public int attackDefense;
    public int magicDefense;
    public int speed;
}
