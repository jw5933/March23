using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    public GameObject book;
    SpriteRenderer bookRenderer;
    SpriteRenderer myRenderer;
    // Start is called before the first frame update
    void Start()
    {
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        if(book !=null){
            book.GetComponent<SpriteRenderer>().sortingOrder = myRenderer.sortingOrder -1;
            book.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void showBook(){
        book.SetActive(true);
        book.GetComponent<SpriteRenderer>().sortingOrder = myRenderer.sortingOrder +1;
    }
}
