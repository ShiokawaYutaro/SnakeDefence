using UnityEngine;
using static TrailRecorder;

public class TailFollowManager : MonoBehaviour
{
    static public TailFollowManager instance = null;
    public GameObject tailFollowPrefab;  // TailFollowプレハブをインスペクターでセット
    public Transform tailParent;         // TailFollowを置く親（例えば自分の子など）
    private TrailRecorder trailRecorder;

    private int followCount = 1; // 最低1つは存在

    void Start()
    {
        instance = this;
        trailRecorder = transform.GetComponent<TrailRecorder>();
        AddTrail(5);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }
    }

    public void AddTrail(int trailCount)
    {
        for (int i = 0,max = trailCount; i < max; i++)
        {
            // TrailRecorderに新しいトレイル追加
            trailRecorder.AddTrail();

            // TailFollowの新しいオブジェクトを生成して親にセット
            GameObject newTail = Instantiate(tailFollowPrefab, tailParent);
            TailFollow tailFollowComp = newTail.GetComponent<TailFollow>();

            // 新しいTailFollowに追従するトレイル番号をセット
            tailFollowComp.trailNumber = followCount;

            // 配列のインデックスに合わせて追従番号を調整（任意でfollowIndexも変えると良い）
            tailFollowComp.followIndex = followCount; // 例えば少し間隔を変える

            followCount++;
        }
        
    }
}

