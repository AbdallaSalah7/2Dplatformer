using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Rendering.Universal.Light2D;

public class StickyWall : MonoBehaviour
{
    //its here 
    public bool isSticky = false;
    //public Sprite stickySprite;
    public float stickTime = 2f;
    private Tilemap stickyMap;
    public Tile stickyTileRight;
    public Tile stickyTileLeft;
    public bool DrawRight {get; private set;}
    public bool DrawLeft {get; private set;}    

    public bool canDraw;
    //public Tile[] stickyTiles;
    
    List<GameObject> glowLights;

    //[SerializeField] UnityEngine.Rendering.Universal.Light2D glow;
    [SerializeField] GameObject glowSlime;

    // Start is called before the first frame update
    void Start()
    {
        stickyMap = GetComponent<Tilemap>();
        DrawRight = false;
        DrawLeft = false;
        canDraw = true;
        isSticky = false;

        glowSlime.gameObject.SetActive(false);
        glowLights = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(glowLights.Count == 0)
            isSticky = false;
    }

    public void DrawRightTile(Vector3Int location){

        /* stickyMap.SetTile(location, stickyTileRight);

        print("hit msg");
        isSticky = true;
        //Invoke("MakeUnsticky", stickTime);
        StartCoroutine(removeTile(location)); */

        if(stickyMap.HasTile(location)){

            canDraw = false;
            return;
        }
        else{

            stickyMap.SetTile(location, stickyTileRight);

            canDraw = true;
            isSticky = true;
            //Invoke("MakeUnsticky", stickTime);
            StartCoroutine(removeTile(location));  
        }
    }


    
    public void DrawLeftTile(Vector3Int location){

        if(stickyMap.HasTile(location)){

            canDraw = false;
            return;
        }
        else{

            stickyMap.SetTile(location, stickyTileLeft);

            canDraw = true;
            isSticky = true;
            //Invoke("MakeUnsticky", stickTime);
            StartCoroutine(removeTile(location));  
        }
    }



    public void MakeStickyGlow(Vector3 loc){

        if(!canDraw)
            return;

        GameObject lit = Instantiate(glowSlime, new Vector3(loc.x - 0.5f, loc.y-0.1f, loc.z), Quaternion.identity);
        glowLights.Add(lit);
        lit.gameObject.SetActive(true);
    }




    IEnumerator removeTile(Vector3Int location)
    {
        //stickyMap.DeleteCells(new Vector3Int(0,0,0), 1, 1, 1);
        yield return new WaitForSeconds(stickTime);
        
        Destroy(glowLights[0]);
        glowLights.RemoveAt(0);

        stickyMap.SetTile(location, null);
        
        isSticky = false;
    }

    /* void MakeUnsticky(Vector3Int location)
    {
        //stickyMap.DeleteCells(new Vector3Int(0,0,0), 1, 1, 1);
        stickyMap.SetTile(location, null);
        isSticky = false;
    } */

    /* public void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")){
            //print("enter");

            if(Input.GetButtonDown("Jump")){
                //isSticky = false;
                other.gameObject.SendMessage("Jump");
                //print("test");
                
            }

        }
    } */
}




