using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private List<MapData> _mapDataList = null;
    [SerializeField] private List<GameObject> _mapObjectList = null;

    private List<MapObject> _useMapList = null;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _mapDataList = new List<MapData>();
        _mapObjectList = new List<GameObject>();
        //for (int i = 0; i < _mapDataList.Count; i++)
        //{
        //    _mapDataList[i].ID = i;
        //}

        //AddMap(0);
    }

    public void AddMap(int ID)
    {
        GameObject map = Instantiate(_mapObjectList[ID].gameObject);
    }
}
