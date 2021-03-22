using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //variables used in this script
    #region VARIABLES
    //reference to gamemanager
    public GameManager gameManager;

    //player variables
    Rigidbody2D myBody;
    Animator myAnim;
    SpriteRenderer myRenderer;

    //mouse variables
    [SerializeField] Vector3 mousePos;
    [SerializeField]bool isDragging = false;
    [SerializeField]bool letGo = true;
    float yPos; //drop currect item where player was standing

    //NPC conditions variables
    public float sightDist;
   [SerializeField] private bool hitNPC = false;
    public float distanceFromNPC;

    //for Raycast
    // public LayerMask activatableLayer;
    // public LayerMask NPCLayer;

    //the object the mouse clicks on
    GameObject hitnpc;
    GameObject hitobj;
    //the object the mouse clicked on
    [SerializeField] GameObject currentObj;
    [SerializeField] GameObject currentNPC;

    //movement variables
    float moveDirX;
    float moveDirY;
    public float speed;
    public float speedLim = 0.7f;

    //game system variables
    [SerializeField]int numOfBooks;
    public bool key = false;
    [SerializeField]int numOfQuests;
    #endregion

    //strings for textboxes (NPC)
    #region NPCSTRINGS
    string collectedBook = "You've picked up a book! Return it to the Librarian.";
    string cannotCollectBook = "Hm.. this doesn't look like any book you're looking for...";

    string maxedQuestsText = "You seem busy at the moment, come back later.";
    //NPC strings
    string lyingManText = "Last I remember I was talking to my friend, Lancer...\nHe's very tall. He said he was going to go look at\nsome astronomy books, west of the lounge.";
    bool lyingMan = false;
    bool lyingManQuest = false;

    string fancyManText = "Find the short man wearing a\nsuit. He's got a beard and mustache.\nBe persistent when you find him!";
    bool fancyMan = false;

    string poshManText = "I'm locked out! The rabbit that\nstole my keys ran north.";
    bool poshMan = false;
    bool rabbit = false;

    string tallManText = "We were in the study together, so I\nthink he put it down on the table and\nit probably fell off and was hidden behind something.";
    bool tallMan = false;

    string shortManText = "Come back later..";
    int shortManNum = 0;
    bool shortMan = false;

    string womanText = "I left mine up northeast, when I\nwas placing down flowers.";
    bool woman = false;

    #endregion


//-------------------- METHODS -----------------\\
    //set gameobjects body,anim, and render
    void Start(){
        myBody = gameObject.GetComponent<Rigidbody2D>(); 
        myAnim = gameObject.GetComponent<Animator>();
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    //update movement
    void FixedUpdate(){
        move();
    }

    //update everything else
    void Update(){
        systems();

        if(isDragging && currentObj != null && currentObj.tag == "Draggable"){
            Debug.Log(currentObj.name);
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - currentObj.gameObject.GetComponent<Renderer>().bounds.center;
            currentObj.transform.Translate(mousePosition);
            letGo = false;
            yPos = transform.position.y;
        }

        if(letGo && currentObj!=null && currentObj.tag == "Draggable"){
            if(currentObj.transform.position.y > yPos){
                transform.Translate(Vector3.up*moveDirY*Time.deltaTime*speed);
                currentObj.transform.Translate(Vector3.down*Time.deltaTime*16.4f);
                currentObj.gameObject.GetComponent<Renderer>().sortingOrder = -(int)Mathf.Floor(currentObj.gameObject.transform.position.y);
            }
            else if(currentObj.transform.position.y <= yPos){
                currentObj = null;
            }
            else{
                letGo = false;
            }
        }
    }

    //system management: mouse, keyboard input
    void systems(){
        //update player's depth
        myRenderer.sortingOrder = -(int)Mathf.Floor(gameObject.transform.position.y);
        
        //get the mouse position
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        //Debug.DrawLine(transform.position, mousePos, Color.red, 1/60f);
        

        //check if the mouse clicked on any objects
        if(Input.GetMouseButtonDown(0)){
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), mousePos);

            if(hit.collider != null){
                //Debug.Log(hit.collider.gameObject.name);
                if(hit.collider.gameObject.tag =="NPC"){
                    checkNPC(hit.collider.gameObject);
                }
                else if(hit.collider.gameObject.tag =="Activatable"){
                    checkActivatable(hit.collider.gameObject);
                }
                 else if(hit.collider.gameObject.tag =="Draggable"){
                     if(Vector3.Distance(transform.position, hit.collider.gameObject.transform.position) <= sightDist){
                        currentObj = hit.collider.gameObject;
                        if(currentObj.GetComponent<item>() != null){
                            currentObj.GetComponent<item>().showBook();
                        }
                    }
                }
            } 
        }

        if(currentObj != null && currentObj.gameObject.tag == "Draggable"){
            checkMouseDrag();
        }

        //if the player gets too far from the npc, close text
        if(hitNPC){
            if(Vector3.Distance(transform.position, currentNPC.transform.position) >= distanceFromNPC){
                // gameManager.dialogue.Clear();
                gameManager.CloseTextBoxes();
                hitNPC = false;
            }
        }

        //open inventory
        if(Input.GetKey(KeyCode.I)){
            gameManager.ShowInventory("Inventory \n---------\nBooks: "+ numOfBooks);
            if(key){
                gameManager.ShowInventory(gameManager.inventoryText.text + "\nA key");
            }
            if(gameManager.booksLeft == 0){
                gameManager.ShowInventory(gameManager.inventoryText.text + "\nSnacks :D");
            }
        }

        //open quest
        if(Input.GetKey(KeyCode.Q)){
            gameManager.ShowQuests("Quest Information\n---------\n");
            if(lyingMan && !tallMan && lyingManQuest){
            gameManager.ShowQuests(gameManager.questText.text + "Man on floor: " + lyingManText+"\n\n");
            }
            if(poshMan && !key){ //key
            gameManager.ShowQuests(gameManager.questText.text + "Posh man: " + poshManText+"\n\n");
            }
            if(fancyMan && shortManNum<2){
            gameManager.ShowQuests(gameManager.questText.text + "Fancy man: " + fancyManText+"\n\n");
            }
            if(tallMan && lyingMan && lyingManQuest){
            gameManager.ShowQuests(gameManager.questText.text + "Tall man (Lancer): " + tallManText+"\n\n");
            gameManager.ShowQuests(gameManager.questText.text + "Man on floor (Winston): Come back to me after!\n\n");
            }
            if(shortMan && shortMan && shortManNum<2){
            gameManager.ShowQuests(gameManager.questText.text + "Short man: " + shortManText+"\n\n");
            }
            if(woman){
            gameManager.ShowQuests(gameManager.questText.text + "Woman: " + womanText+"\n\n");
            }

        }

        //close npc and quest boxes
        if(!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.Q)){
            gameManager.CloseInfoBoxes();
        }
    }

    void checkMouseDrag(){
        if (Input.GetMouseButton(0)){
            isDragging = true;
        }
        else{
            isDragging = false;
            if(currentObj!=null && currentObj.tag == "Draggable"){
            letGo = true;
            }
        }
                    
    }

    //check if the object clicked by the mouse is activatable
    void checkActivatable(GameObject hitobj){
        currentObj = hitobj;
        if(Vector3.Distance(transform.position, hitobj.transform.position)<= sightDist){
            displayActivatableText(currentObj.name);
        }
    }
    //display text activated by the object
    void displayActivatableText(string name){
        if(name == "FlowerPot"){
            currentObj.GetComponent<flowerPot>().BreakPot();
        }
        else if(name == "Lock"){
            if(key){
                 Destroy(currentObj);
            }
        }
        else{
            switch(name){
                case "Book1":
                    if(lyingMan && tallMan){
                        gameManager.dialogue.Add(collectedBook);
                        Destroy(currentObj);
                        numOfBooks++;
                        numOfQuests--;
                    }
                    else{
                        gameManager.dialogue.Add(cannotCollectBook);
                    }
                break;
                case "Book2":
                    if(poshMan){
                        gameManager.dialogue.Add(collectedBook);
                        Destroy(currentObj);
                        numOfBooks++;
                        numOfQuests--;
                    }
                    else{
                        gameManager.dialogue.Add(cannotCollectBook);
                    }
                break;
                case "Book3":
                    if(woman){
                        gameManager.dialogue.Add(collectedBook);
                        Destroy(currentObj);
                        woman = false;
                        numOfBooks++;
                        numOfQuests--;
                        Destroy(GameObject.Find("NPCs/Woman"));
                    }
                    else{
                        gameManager.dialogue.Add(cannotCollectBook);
                    }
                break;
            }
        StartCoroutine(gameManager.ShowNPCText());
        }
        
    }


    //check if the object clicked by the mouse is an NPC
    void checkNPC(GameObject hitnpc){
        if(Vector3.Distance(transform.position, hitnpc.transform.position)<= sightDist){
            gameManager.CloseTextBoxes();
            currentNPC = hitnpc;
            displayNPCText(currentNPC.name);
            hitNPC = true;
        }
    }

    //display NPC's dialogue
    void displayNPCText(string npc){
        gameManager.CloseTextBoxes();
        if(npc == "Librarian"){
            if(gameManager.booksLeft == gameManager.maxBooks && numOfBooks==0){
                    gameManager.dialogue.Add("Hey Kid! Help me out with something and I'll give you some snacks.");
                    gameManager.dialogue.Add("I'm missing "+ gameManager.booksLeft + " books that were borrowed by some people in this library.");
                    gameManager.dialogue.Add("Find them and their books and bring the books back to me.");
                    // StartCoroutine(gameManager.ShowNPCText());
                    // gameManager.ShowNPCText("Hey kid! Bring me "+ gameManager.booksLeft + " books, and I'll give you some snacks.");
                }
                else{
                    if(numOfBooks > 0){
                        gameManager.booksLeft = gameManager.booksLeft - numOfBooks;
                        if(gameManager.booksLeft == 0){
                            gameManager.dialogue.Add("Thanks kid! Here, have some yummy snacks!");
                            // Time.timeScale = 0;
                        }
                        else{
                            gameManager.dialogue.Add("Thanks for the " + numOfBooks + ((numOfBooks==1)? " book":" books") + "!\nBring me the other "+ gameManager.booksLeft + ((gameManager.booksLeft<=1)? " book":" books") + " and I'll give you some snacks. :)");
                        }
                        numOfBooks = 0;
                    }
                    else{
                        if(gameManager.booksLeft == 0){
                            gameManager.dialogue.Add("Thanks kid! Here, have some yummy snacks!");
                            // Time.timeScale = 0;
                        }
                        gameManager.dialogue.Add("Find and bring me the "+ gameManager.booksLeft + " other" + ((gameManager.booksLeft<=1)? " book!":" books!"));
                        gameManager.dialogue.Add("I've got some delicious snacks waiting for you!");
                        
                        // StartCoroutine(gameManager.ShowNPCText());
                        // gameManager.ShowNPCText("Bring me "+ gameManager.booksLeft + " more books!\nI've got some delicious snacks waiting for you!");
                    }
                }
        }
        else if (numOfQuests < gameManager.maxQuests){
            switch(npc){
                case "LyingMan":
                if (!lyingMan && !lyingManQuest){
                    numOfQuests++;
                    lyingMan = true;
                    lyingManQuest = true;
                    gameManager.dialogue.Add("Hmm..? Huh? Oh my book...? Oh shoot where is it...");
                    gameManager.dialogue.Add("Last I remember I was talking to my friend, Lancer...");
                    gameManager.dialogue.Add("He's very tall. He said he was going to go look at some astronomy books, west of the lounge.");
                    gameManager.dialogue.Add("Let me know once you've found him!");
                }
                else{
                    if(tallMan){
                        gameManager.dialogue.Add("Thanks for returning the book for me!");
                        gameManager.dialogue.Add("I think there was someone at the lounge who was looking for their book earlier. Maybe they're still there.");
                        lyingManQuest = false;
                    }
                }
                    break;
                case "PoshMan": //guy missing key
                if (!poshMan){
                    numOfQuests++;
                    poshMan = true;
                    gameManager.dialogue.Add("My book's in here but I'm locked out!");
                    gameManager.dialogue.Add("A rabbit stole my keys while I was browsing the shelves. I think I saw it run north!");
                }else{
                    if(rabbit){
                        gameManager.dialogue.Add("Oh thank you thank you! I was worried the librarian might think poorly of me because of this...");
                    }
                }
                    break;
                case "FancyMan":
                    if (!fancyMan){
                    numOfQuests++;
                    fancyMan = true;
                    gameManager.dialogue.Add("I let someone borrow my book, but now I can't find him!");
                    gameManager.dialogue.Add("He's a short man wearing a suit. He's got a beard and mustache.");
                    gameManager.dialogue.Add("If you find him, be persistent!");
                }else{
                    if (shortManNum==2){
                        gameManager.dialogue.Add("Thank you for returning my book kid!");
                    }
                }
                    break;
                case "Woman":
                    if (!woman){
                    numOfQuests++;
                    woman = true;
                    gameManager.dialogue.Add("Books? Oh shoot, I left mine up northeast, when I was placing down flowers!");
                    gameManager.dialogue.Add("Would you return it for me? I'm in a bit of a hurry! Thanks kid!");
                }
                    break;
                case "TallMan":
                    gameManager.dialogue.Add("Hm? Oh I didn't see you down there. Be careful.");
                    break;
                case "ShortMan":
                    gameManager.dialogue.Add("Shhhhhh. I'm reading here!");
                    break;
                case "Rabbit":
                    gameManager.dialogue.Add("What's the time?");
                    break;
                default:
                    gameManager.dialogue.Add("I'm busy right now!");
                    break;
            }
        }
        else if(numOfQuests >= gameManager.maxQuests){
            switch(npc){
                case "TallMan":
                    if (lyingMan){
                    tallMan = true;
                    gameManager.dialogue.Add("You're looking for Winston's book? Hmm.. Let me think...");
                    gameManager.dialogue.Add("We were in the study together, so I think he put it down on the table and it probably fell off and was hidden behind something.");
                    gameManager.dialogue.Add("Check around there. In the meantime, I've got a book on me that I forgot to return, so here you go.");
                    numOfBooks++;
                }else{
                    gameManager.dialogue.Add("Hm? Oh I didn't see you down there. Be careful.");
                }
                    break;
                case "ShortMan":
                    if (!fancyMan){
                    gameManager.dialogue.Add("Shhhhhh. I'm reading here!");
                    }
                    else{
                        if(shortManNum == 2){
                            gameManager.dialogue.Add("Alright. Alright. You can have the book.");
                            numOfBooks++;
                            numOfQuests--;
                        }
                        else{
                            shortMan = true;
                            shortManNum++;
                            gameManager.dialogue.Add("I'm still reading it! Could you come back later...");
                        }
                    }
                    break;
                case "Rabbit":
                    if (!rabbit){
                    if(poshMan){
                        rabbit = true;
                        gameManager.dialogue.Add("Do you have the time? What? A key?\nThis key? Fine, you can have it.");
                        key = true;
                    }
                    else{
                        gameManager.dialogue.Add("What's the time?");
                    }
                }
                    break;
                default:
                    gameManager.dialogue.Add(maxedQuestsText);
                    break;
            }
        }
        StartCoroutine(gameManager.ShowNPCText());
    }

    //movement code
    void move(){
        //reset animation
        resetAnim();
        //get the directional movement of the player
        moveDirX = Input.GetAxis("Horizontal");
        moveDirY = Input.GetAxis("Vertical");

        //if the player is moving right
        if(moveDirX > 0){
            myRenderer.flipX = false;
            myAnim.SetBool("walkingSide",true);
        }
        //if player is moving left
        else if(moveDirX < 0){
            myRenderer.flipX = true;
            myAnim.SetBool("walkingSide",true);
        }

        //horizontal speed
        if (moveDirX !=0 && moveDirY !=0){
            moveDirX *= speedLim;
            moveDirY *= speedLim;
        }

        //movement
        transform.Translate(Vector3.up*moveDirY*Time.deltaTime*speed);
        transform.Translate(Vector3.right*moveDirX*Time.deltaTime*speed);
        //myBody.velocity = new Vector2(moveDirX*speed, moveDirY*speed);
    }

    //reset the animation
    void resetAnim(){
        myAnim.SetBool("walkingSide",false);
    }
}
