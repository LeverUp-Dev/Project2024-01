using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;

public enum pathType
{
    main, h, v
}

public class TilePathManager : MonoBehaviour
{
    [Header("Tile")]
    [SerializeField] private Transform[] mainTileGroup;
    [SerializeField] private Transform[] hTileGroup;
    [SerializeField] private Transform[] vTileGroup;

    [Header("Player")]
    public PlayerController pController;

    [Header("Tile Path")]
    public pathType path;

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

    [Header("Serializable Tile")]
    public List<DataMainTilePath> mainTiles = new List<DataMainTilePath>();
    public List<DataHTilePath> hTiles = new List<DataHTilePath>();
    public List<DataVTilePath> vTiles = new List<DataVTilePath>();

    private void Awake()
    {
        //타일 경로 초기화
        foreach (Transform t in mainTileGroup)
        {
            mainTiles.Add(new DataMainTilePath(t));
        }
        mainTiles[5].isChoiceTile = true;
        mainTiles[35].isChoiceTile = true;
        foreach (Transform t in hTileGroup)
        {
            hTiles.Add(new DataHTilePath(t));
        }
        hTiles[4].isChoiceTile = true;
        foreach (Transform t in vTileGroup)
        {
            vTiles.Add(new DataVTilePath(t));
        }
        vTiles[4].isChoiceTile = true;

        //경로 설정
        path = pathType.main;
    }

    public List<DataMainTilePath> GetMainTilePath()
    {
        return mainTiles;
    }

    public List<DataHTilePath> GetHTilePath()
    {
        return hTiles;
    }

    public List<DataVTilePath> GetVTilePath()
    {
        return vTiles;
    }

    public Transform GetTilePath(int tileIndex)
    {
        if(path == pathType.main)
        {
            return GetMainTilePath()[tileIndex].mainTile;
        }
        else if(path == pathType.h)
        {
            return GetHTilePath()[tileIndex].hTile;
        }
        else
        {
            return GetVTilePath()[tileIndex].vTile;
        }
    }

    public void ChangeTilePathMain(GameObject choicePathUI)
    {
        path = pathType.main;

        choicePathUI.SetActive(false);
    }

    public void ChangeTilePathH(GameObject choicePathUI)
    {
        path = pathType.h;

        if(pController.currentTile == 5)
            pController.currentTile = -1;

        choicePathUI.SetActive(false);
    }

    public void ChangeTilePathV(GameObject choicePathUI)
    {
        path = pathType.v;

        if(pController.currentTile == 35)
            pController.currentTile = -1;

        choicePathUI.SetActive(false);
    }

    public void NotChangeTilePath(GameObject choicePathUI)
    {
        choicePathUI.SetActive(false);
    }
}
