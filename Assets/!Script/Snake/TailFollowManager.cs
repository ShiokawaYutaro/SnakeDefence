using UnityEngine;
using static TrailRecorder;

public class TailFollowManager : MonoBehaviour
{
    static public TailFollowManager instance = null;
    public GameObject tailFollowPrefab;  // TailFollow�v���n�u���C���X�y�N�^�[�ŃZ�b�g
    public Transform tailParent;         // TailFollow��u���e�i�Ⴆ�Ύ����̎q�Ȃǁj
    private TrailRecorder trailRecorder;

    private int followCount = 1; // �Œ�1�͑���

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
            // TrailRecorder�ɐV�����g���C���ǉ�
            trailRecorder.AddTrail();

            // TailFollow�̐V�����I�u�W�F�N�g�𐶐����Đe�ɃZ�b�g
            GameObject newTail = Instantiate(tailFollowPrefab, tailParent);
            TailFollow tailFollowComp = newTail.GetComponent<TailFollow>();

            // �V����TailFollow�ɒǏ]����g���C���ԍ����Z�b�g
            tailFollowComp.trailNumber = followCount;

            // �z��̃C���f�b�N�X�ɍ��킹�ĒǏ]�ԍ��𒲐��i�C�ӂ�followIndex���ς���Ɨǂ��j
            tailFollowComp.followIndex = followCount; // �Ⴆ�Ώ����Ԋu��ς���

            followCount++;
        }
        
    }
}

