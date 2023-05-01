using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StickyWall : MonoBehaviour
{
    //its here 
    public bool isSticky = false; //want meeting? u free? yea ? ok if u want no if you want yea tmam wait wait phone call take your time 
    //public Sprite stickySprite;
    public float stickTime = 2f;
    private Tilemap stickyMap;
    public Tile stickyTileRight;
    public Tile stickyTileLeft;
    public bool DrawRight {get; private set;}
    public bool DrawLeft {get; private set;}    
    public Tile[] stickyTiles;
    public Vector3Int loc;

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
        //Invoke("MakeUnsticky", stickTime);
        StartCoroutine(removeTile(location));
        
    }
    public void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")){
            print("enter");

            if(Input.GetButtonDown("Jump")){
                isSticky = false;
                other.gameObject.SendMessage("GiveJump");
                print("test");
                
            }

        }
    }
    public void DrawLeftTile(Vector3Int location){

        stickyMap.SetTile(location, stickyTileLeft);
        isSticky = true;
        //Invoke("MakeUnsticky", stickTime);
        StartCoroutine(removeTile(location));
    }

    IEnumerator removeTile(Vector3Int location)
    {
        //stickyMap.DeleteCells(new Vector3Int(0,0,0), 1, 1, 1);
        yield return new WaitForSeconds(stickTime);
        stickyMap.SetTile(location, null);
        isSticky = false;
    }

    /* void MakeUnsticky(Vector3Int location)
    {
        //stickyMap.DeleteCells(new Vector3Int(0,0,0), 1, 1, 1);
        stickyMap.SetTile(location, null);
        isSticky = false;
    } */
}

//ok so i thought what if we do an array just like the one that checks is player is stuck in sticky but of type tile tmam, then idk where is the timer of the sticky but basically removed that and each tile we draw add to array then after some time remove from array and delete hmmm 


