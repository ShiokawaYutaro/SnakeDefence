using System.Collections.Generic;
using UnityEngine;

public class TrailRecorder : MonoBehaviour
{
    public struct TrailPoint
    {
        public Vector3 position;
        public Quaternion rotation;

        public TrailPoint(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }

    public List<List<TrailPoint>> trails = new List<List<TrailPoint>>();
    private float recordSpacing = 0.4f;

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
        // �ŏ��̃g���C����p��
        trails.Add(new List<TrailPoint>());
        trails[0].Add(new TrailPoint(transform.position, transform.rotation));
    }

    void Update()
    {
        if (Vector3.Distance(lastPosition, transform.position) >= recordSpacing)
        {
            lastPosition = transform.position;

            for (int i = 0; i < trails.Count; i++)
            {
                trails[i].Insert(0, new TrailPoint(transform.position, transform.rotation));

                if (trails[i].Count > 500)
                    trails[i].RemoveAt(trails[i].Count - 1);
            }
        }
    }


    // �V�����g���C���ǉ��p���\�b�h
    public void AddTrail()
    {
        List<TrailPoint> newTrail = new List<TrailPoint>();

        // �����Ǐ]�p�̃_�~�[�f�[�^�i20�Ȃǁj
        for (int i = 0; i < 20; i++)
        {
            newTrail.Add(new TrailPoint(transform.position, transform.rotation));
        }

        trails.Add(newTrail);
    }
}
