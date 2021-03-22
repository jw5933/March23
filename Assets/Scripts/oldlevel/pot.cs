using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pot : MonoBehaviour
{
    public string NPC;
    public bool isBookVisible = false;
    public bool notDestroyed = true;
    public bool canDestroy = false;

    public Sprite brokenPot;

    public GameObject book;
    SpriteRenderer bookRenderer;

    SpriteRenderer myRenderer;
    // Start is called before the first frame update
    void Start()
    {
        bookRenderer = (SpriteRenderer)book.GetComponent<SpriteRenderer>();
        bookRenderer.enabled = false;

        myRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(book == null){
            notDestroyed = false;
        }

        if (isBookVisible && notDestroyed){
            bookRenderer.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "player" && canDestroy){
            //pot potScript = (pot)other.gameObject.GetComponent(typeof(pot));
            //SpriteRenderer potSpr = (SpriteRenderer)other.gameObject.GetComponent<SpriteRenderer>();
            myRenderer.sprite = brokenPot;
            isBookVisible = true;
        }

    }
}
