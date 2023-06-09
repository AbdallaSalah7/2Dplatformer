using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StickPlatform : MonoBehaviour
{

    public bool rightHit;
    public bool leftHit;
    public bool topHit;
    public bool bottomHit;
    public Vector3Int location;
    
    //Sticky tile sprite
    public Sprite stickySprite;
    
    //Tilemaps
    public Tilemap targetTilemap;
    public StickyWall stickyTilemap;

    //individual tiles
    //public Tile stickyTileRightHit;
    //public Tile stickyTileLeftHit;

    // Start is called before the first frame update
    void Start()
    {
        targetTilemap = GetComponent<Tilemap>();
        //stickyTilemap = GetComponent<DrawStickyOnTIlemap>();
        rightHit = false;
        leftHit = false;
        topHit = false;
        bottomHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("StickyBullet"))
        {
            
            Vector3 point = other.gameObject.transform.position;

            if(rightHit){

               location = targetTilemap.WorldToCell(new Vector3(point.x, point.y, point.z));
               stickyTilemap.SendMessage("DrawRightTile", location);
               stickyTilemap.SendMessage("MakeStickyGlow", point);

               print("hit right");
            }
            
            else if(leftHit){

               location = targetTilemap.WorldToCell(new Vector3(point.x-1, point.y, point.z)); 
               stickyTilemap.SendMessage("DrawLeftTile", location);
               stickyTilemap.SendMessage("MakeStickyGlow", point);


               print("hit left");

            }

            //print("point: " + location);
            
            //stickyTilemap.SetTile(location, stickyTileRightHit);
            
        }
    }


    public void SetBulletDirectionRight() {

        rightHit = true;
        leftHit = false;
        topHit = false;
        bottomHit = false;
    }

    public void SetBulletDirectionLeft(){

        leftHit = true;
        rightHit = false;
        topHit = false;
        bottomHit = false;
    }
}


