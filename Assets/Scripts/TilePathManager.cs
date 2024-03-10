using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePathManager : MonoBehaviour
{

    public GameObject mainTileGroup;

    [System.Serializable]
    public class DataTilePath {

        public Transform mainTiles;

        public DataTilePath(Transform mainTiles)
        {
            this.mainTiles = mainTiles;
        }
    }

    public List<DataTilePath> managerTiles = new List<DataTilePath>();

    void Awake()
    {
        for (int i = 0; i < mainTileGroup.transform.childCount; i++)
        {
            managerTiles.Add(new DataTilePath(mainTileGroup.GetComponentsInChildren<Transform>()[i + 1]));
        }
    }
}
