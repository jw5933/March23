using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerPot : MonoBehaviour
{
    public bool isBookVisible = false;
    public bool notDestroyed = true;
    BoxCollider2D myCollider;

    public Sprite brokenPot;

    public GameObject book;
    SpriteRenderer bookRenderer;

    SpriteRenderer myRenderer;


    void Start()
    {
        myCollider = gameObject.GetComponent<BoxCollider2D>();
        myRenderer = gameObject.GetComponent<SpriteRenderer>();

        if(book !=null){
            book.GetComponent<SpriteRenderer>().sortingOrder = myRenderer.sortingOrder -1;
            book.SetActive(false);
        }
        
    }

    void Update()
    {
        if(book == null){
            notDestroyed = false;
        }

        if (isBookVisible && notDestroyed){
            book.SetActive(true);
            book.GetComponent<SpriteRenderer>().sortingOrder = myRenderer.sortingOrder +1;
        }
    }

    public void BreakPot(){
        myRenderer.sprite = brokenPot;
        myCollider.enabled = false;
        isBookVisible = true;

    }
}
