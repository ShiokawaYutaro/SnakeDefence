using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    static private List<MapData> _mapDataList = new List<MapData>();
    [SerializeField] private List<GameObject> _mapObjectList = new List<GameObject>();


    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        for (int i = 0; i < _mapDataList.Count; i++)
        {
            if (_mapDataList[i].StageLVL == 1)
            {
                Instantiate(_mapObjectList[i]);
            }
        }
    }

}
