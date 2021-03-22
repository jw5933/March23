using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableAssets : MonoBehaviour
{
    public SpriteRenderer text;
    public SpriteRenderer text2;

    public GameObject asset;
    
    // Start is called before the first frame update
    void Start()
    {
        text.enabled = false;
        if(text2 != null){
            text2.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        text.enabled = true;
        if(text2 != null){
            text2.enabled = true;
        }

        if (other.gameObject.name == "player"){
            if(asset !=null){
                pot assetScript = (pot)asset.GetComponent(typeof(pot));
                assetScript.canDestroy = true;
            }
        }
    }
}
