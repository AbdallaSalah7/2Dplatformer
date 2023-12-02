using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructableTiles : MonoBehaviour
{
    public Tilemap destructableTilemap;

    private void Start() {
        destructableTilemap = GetComponent<Tilemap>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            Vector3 hitPosition = Vector3.zero;
            foreach(ContactPoint2D hit in other.contacts){
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                destructableTilemap.SetTile(destructableTilemap.WorldToCell(hitPosition), null);
            }
        }
    }

    IEnumerator Wait(){
        yield return new WaitForSeconds(5);
        
    }


}
