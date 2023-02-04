using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObserver : MonoBehaviour
{
    [SerializeField]
    private List<TilesList> allTiles;
}

[Serializable]
public class TilesList
{
    public Tile tile1;
    public Tile tile2;
    public Tile tile3;
}
