using System.IO;
using UnityEngine;

using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerStatusData
{
    public int coin;
    public float attack;
    public float defense;
    public float maxHp;

    public PlayerStatusData(int money, float attack, float defense, float maxHp)
    {
        this.coin = money;
        this.attack = attack;
        this.defense = defense;
        this.maxHp = maxHp;
    }
}


public class PlayerSaveManager
{
    private static string SavePath => Application.persistentDataPath + "/player_stats.json";

    public static readonly string LobbyScene = "Lobby";
    public static readonly string GameScene = "Stage";

    public static void SaveFromLobby(int coin, float attack, float defense, float maxHp)
    {
        var data = new PlayerStatusData(coin, attack, defense, maxHp);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    // バトルで使う保存（コインだけ保存）
    public static void SaveFromBattle(int coin)
    {
        PlayerStatusData data;

        string json = File.ReadAllText(SavePath);
        data = JsonUtility.FromJson<PlayerStatusData>(json);

        data.coin = coin; // コインだけ更新

        string saveJson = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, saveJson);
    }

    public static PlayerStatusData Load(int defaultCoin, float defaultAttack, float defaultDefense, float defaultMaxHp)
    {
        if (!File.Exists(SavePath))
        {
            // ファイルが無ければ「初回ロビーの値」を使う
            var initData = new PlayerStatusData(defaultCoin, defaultAttack, defaultDefense, defaultMaxHp);

            // すぐ保存して以降のシーンでも使えるようにする
            string json = JsonUtility.ToJson(initData);
            File.WriteAllText(SavePath, json);

            Debug.Log("初回ロビー: プレイヤーの初期値で保存しました");
            return initData;
        }

        // ファイルがあれば保存済みを返す
        string data = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<PlayerStatusData>(data);
    }


    public static void ResetData()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("セーブデータを削除しました");
        }
    }
}
