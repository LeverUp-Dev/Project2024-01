using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePathManager : MonoBehaviour
{
    public Transform[] mainTileGroup;
    public Transform[] hTileGroup;
    public Transform[] vTileGroup;

    public class TileStatus
    {
        public bool isChoiceTile;
    }

    [System.Serializable]
    public class DataMainTilePath : TileStatus
    {
        public Transform mainTile;

        public DataMainTilePath(Transform mainTiles)
        {
            this.mainTile = mainTiles;
        }
    }

    [System.Serializable]
    public class DataHTilePath : TileStatus
    {
        public Transform hTile;

        public DataHTilePath(Transform hTiles)
        {
            this.hTile = hTiles;
        }
    }

    [System.Serializable]
    public class DataVTilePath : TileStatus
    {
        public Transform vTile;

        public DataVTilePath(Transform vTiles)
        {
            this.vTile = vTiles;
        }
    }

    public List<DataMainTilePath> mainTiles = new List<DataMainTilePath>();
    public List<DataHTilePath> hTiles = new List<DataHTilePath>();
    public List<DataVTilePath> vTiles = new List<DataVTilePath>();

    private void Awake()
    {
        foreach (Transform t in mainTileGroup)
        {
            mainTiles.Add(new DataMainTilePath(t));
        }
        mainTiles[5].isChoiceTile = true;
        mainTiles[15].isChoiceTile = true;
        mainTiles[25].isChoiceTile = true;
        mainTiles[35].isChoiceTile = true;

        foreach (Transform t in hTileGroup)
        {
            hTiles.Add(new DataHTilePath(t));
        }
        hTiles[5].isChoiceTile = true;

        foreach (Transform t in vTileGroup)
        {
            vTiles.Add(new DataVTilePath(t));
        }
        hTiles[5].isChoiceTile = true;
    }
}
