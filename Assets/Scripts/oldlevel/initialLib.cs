// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class initialLib : MonoBehaviour
// {
//     public Sprite textNext;
//     public Sprite bookText;
//     public Sprite[] nums;
//     public SpriteRenderer text;
//     public SpriteRenderer book;
//     public SpriteRenderer number;

//     int booksLeft = 6;
//     public GameObject player;
//     public GameObject ending;

//     // Start is called before the first frame update
//     void Start()
//     {
//         number.enabled = false;
//         book.enabled = false;
//     }

//     // Update is called once per frame
//     void Update()
//     {

//         if (booksLeft < 6){
//             text.sprite = textNext;
//             number.enabled = true;
//             book.enabled = true;
//         }

//         switch(booksLeft){
//             case 1: number.sprite = nums[0];
//             book.sprite = bookText;
//             break;
//             case 2: number.sprite = nums[1];
//             break;
//             case 3: number.sprite = nums[2];
//             break;
//             case 4: number.sprite = nums[3];
//             break;
//             case 5: number.sprite = nums[4];
//             break;
//         }

//         if(booksLeft == 0){
//             text.enabled = false;
//             number.enabled = false;
//             book.enabled = false;

//             Time.timeScale = 0;

//             if(GameObject.Find("ending(Clone)")==null){
//                 Instantiate(ending, gameObject.transform.position, Quaternion.identity);
//             }
            
//         }
//     }

//     private void OnCollisionEnter2D(Collision2D other)
//     {
//         if(other.gameObject.name == "player"){
//             checkPlayer();
//         }
//     }

//     // private void OnTriggerEnter2D(Collider2D other)
//     // {
//     //     if(other.gameObject.name == "player"){
//     //         checkPlayer();
//     //     }
//     // }

//     void checkPlayer(){
//         player playerScript = (player)player.GetComponent(typeof(player));
//         if(playerScript.numOfBooks!=0){
//             booksLeft -= playerScript.numOfBooks;
//             playerScript.numOfBooks = 0;
//         }
//     }
// }
