using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StickyWall : MonoBehaviour
{

    public bool isSticky = false;
    //public Sprite stickySprite;
    public float stickTime = 2f;
    private Tilemap stickyMap;
    public Tile stickyTileRight;
    public Tile stickyTileLeft;
    public bool DrawRight {get; private set;}
    public bool DrawLeft {get; private set;}    
    // Start is called before the first frame update
    void Start()
    {
        stickyMap = GetComponent<Tilemap>();
        DrawRight = false;
        DrawLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawRightTile(Vector3Int location){

        stickyMap.SetTile(location, stickyTileRight);
        print("hit msg");
        isSticky = true;
        Invoke("MakeUnsticky", stickTime);
        
    }
    public void DrawLeftTile(Vector3Int location){

        stickyMap.SetTile(location, stickyTileLeft);
        isSticky = true;
        Invoke("MakeUnsticky", stickTime);
    }

    void MakeUnsticky()
    {
        stickyMap.SetTile(new Vector3Int(0,0,0), null);
        isSticky = false;
    }
}


