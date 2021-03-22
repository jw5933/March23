using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<string> dialogue = new List<string>();
    [SerializeField] int letterPerSec;
    [SerializeField] int index;
    bool isTyping;

    public int maxBooks = 5;
    public int maxQuests = 1;
    public int booksLeft;

    public GameObject questTextBox;
    //public GameObject questTextObj;
    public Text questText;

    public GameObject inventoryTextBox;
    //public GameObject inventoryTextObj;
    public Text inventoryText;


    public GameObject dialogueTextBox;
    //public GameObject NPCTextObj;
    public Text dialogueText;
    bool openText = false;

    void Start(){
        ShowStartText();
        booksLeft = maxBooks;
        
        GameObject [] walls = GameObject.FindGameObjectsWithTag("Wall");
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        GameObject[] activatables = GameObject.FindGameObjectsWithTag("Activatable");
        GameObject[] draggables = GameObject.FindGameObjectsWithTag("Draggable");

        
        foreach (GameObject o in npcs){
            o.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)Mathf.Floor(o.transform.position.y);
        }
        foreach (GameObject o in activatables){
            o.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)Mathf.Floor(o.transform.position.y);
        }
        foreach (GameObject o in walls){
            o.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)Mathf.Floor(o.transform.position.y);
        }
        foreach (GameObject o in draggables){
            o.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)Mathf.Floor(o.transform.position.y);
        }
    }
    
    // Update is called once per frame
    void Update(){
        if(openText){
            if(Input.GetKeyDown(KeyCode.Escape)){
                CloseTextBoxes();
            }
            if(Input.GetKeyDown(KeyCode.Space)){
                if(isTyping){
                    dialogueText.text=dialogue[index];
                    isTyping = false;
                }
                else{
                    ++index;
                    if(index < dialogue.Count){
                        StartCoroutine(TypeDialogue(dialogue[index]));
                    }
                }
            }
        }
    }

    public void CloseTextBoxes(){
                if(isTyping){
                    dialogueText.text="";
                    isTyping = false;
                }
                dialogue.Clear();
                index = 0;
                openText = false;
                dialogueTextBox.SetActive(false);
    }

    public void CloseInfoBoxes(){
        questTextBox.SetActive(false);
        inventoryTextBox.SetActive(false);
    }

    private void ShowStartText(){
        dialogueTextBox.SetActive(true);
        dialogue.Add("Welcome! Please press < Space > :)");
        dialogue.Add("< Left Click > an NPC/object when you're near them to interact.");
        dialogue.Add("Hold < I > for inventory & < Q > for quest information.");
        dialogue.Add("Press < ESC > to leave dialogue.");
        
        dialogueText.text = dialogue[index];
        StartCoroutine(TypeDialogue(dialogue[index]));
        
        openText = true;
    }

    public IEnumerator ShowNPCText(){
        yield return new WaitForEndOfFrame();
        dialogueTextBox.SetActive(true);
        dialogueText.text = dialogue[index];
        StartCoroutine(TypeDialogue(dialogue[index]));
        openText = true;
    }

    public void ShowInventory(string text){
        inventoryTextBox.SetActive(true);
        inventoryText.text = text;
    }
    
    public void ShowQuests(string text){
        questTextBox.SetActive(true);
        questText.text = text;
    }

    public IEnumerator TypeDialogue(string str){
        isTyping = true;
        dialogueText.text="";
        foreach(var letter in str.ToCharArray()){
            if(isTyping){
                dialogueText.text +=letter;
                yield return new WaitForSeconds(1f/letterPerSec);
            }
        }
        isTyping = false;
    }
}
