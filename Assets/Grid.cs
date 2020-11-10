using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour
{
    public Tile gridTile;

    private void Start()
    {
        Tilemap tilemap = this.GetComponent<Tilemap>();
        for (int i = 0; i < Map.Singleton.Width; i++)
        {
            for (int j = 0; j < Map.Singleton.Height; j++)
            {
                tilemap.SetTile(new Vector3Int(i,j,0), this.gridTile);
            }
        }
    }

    public void ToggleGrid()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }
}
