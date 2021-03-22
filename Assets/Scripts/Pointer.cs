using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    [SerializeField] private Camera pointerCam;
    [SerializeField] private Sprite pointerSprite;
    [SerializeField] private Sprite arrivedSprite;

    public GameObject nextNPC;
    private RectTransform pointerRectTransform;
    private Vector3 targetPos;
    private Image pointerImage;

    //border for the pointer to not go off screen
    public float borderSize;

    float cameraLeft, cameraRight, cameraBottom, cameraTop;
    

    private void Awake(){
        //targetPos = nextNPC.transform.position;
        targetPos = nextNPC.gameObject.GetComponent<Renderer>().bounds.center;
        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
        pointerImage = transform.Find("Pointer").GetComponent<Image>();
    }

    private void Update()
    {
        Vector2 lowerLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector2 upperRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        cameraLeft = lowerLeft.x;
        cameraBottom = lowerLeft.y;
        cameraRight = upperRight.x;
        cameraTop = upperRight.y;

        //set nextPos and fromPos
        Vector3 nextPos = targetPos;
        Vector3 fromPos = Camera.main.transform.position;
        fromPos.z = 0.0f;

        //get the direction and move pointer relative to that angle
        Vector3 dir = (nextPos - fromPos).normalized;
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) % 360;

        pointerRectTransform.localEulerAngles = new Vector3(0,0,angle);

        //where the target is on screen
        Vector3 targetPosScreenPoint = Camera.main.WorldToScreenPoint(targetPos);
        
        //check if the target is offscreen  
        bool isOffScreen = targetPosScreenPoint.x <= borderSize || targetPosScreenPoint.x >= Screen.width -borderSize|| targetPosScreenPoint.y <= borderSize || targetPosScreenPoint.y >= Screen.height -borderSize;
        //Debug.Log(isOffScreen + " " + targetPosScreenPoint);

        //pointer behaviour if the target is offscreen
        //cap its movement to the edge of the screen
        if(isOffScreen){
            // pointerImage.sprite = pointerSprite;
            // //try 1
            // if (pointerRectTransform.position.x > cameraRight){
            //     pointerRectTransform.position = new Vector3(cameraRight, pointerRectTransform.position.y, pointerRectTransform.position.z);
            // }
            // if(pointerRectTransform.position.x < cameraLeft){
            //     pointerRectTransform.position = new Vector3(cameraLeft, pointerRectTransform.position.y, pointerRectTransform.position.z);
            // }
            // if(pointerRectTransform.position.y > cameraTop){
            //     pointerRectTransform.position = new Vector3(pointerRectTransform.position.x, cameraTop, pointerRectTransform.position.z);
            // }
            // if(pointerRectTransform.position.y < cameraBottom){
            //     pointerRectTransform.position = new Vector3(pointerRectTransform.position.x, cameraBottom, pointerRectTransform.position.z);
            // }

            // //try 2
            // Vector3 cappedPointerScreenPos = targetPosScreenPoint;

            // if (pointerRectTransform.position.x >= cameraLeft){
            //     cappedPointerScreenPos.x = cameraLeft - borderSize;
            // }
            // if(pointerRectTransform.position.x <= cameraRight){
            //     cappedPointerScreenPos.x = cameraRight + borderSize;
            // }
            // if(pointerRectTransform.position.y >= cameraBottom){
            //     cappedPointerScreenPos.y = cameraBottom - borderSize;
            // }
            // if(pointerRectTransform.position.y <= cameraTop){
            //     cappedPointerScreenPos.y = cameraTop + borderSize;
            // }
            // //move the pointer
            // Vector3 pointerWorldPos = pointerCam.ScreenToWorldPoint(cappedPointerScreenPos);
            // pointerRectTransform.position = pointerWorldPos;
            // pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);


            //original
            Vector3 cappedPointerScreenPos = targetPosScreenPoint;
            if (cappedPointerScreenPos.x <= borderSize) cappedPointerScreenPos.x = borderSize;
            if (cappedPointerScreenPos.x >= Screen.width - borderSize) cappedPointerScreenPos.x = Screen.width - borderSize;
            if (cappedPointerScreenPos.y <= borderSize) cappedPointerScreenPos.y = borderSize;
            if (cappedPointerScreenPos.y >= Screen.height - borderSize) cappedPointerScreenPos.y = Screen.height - borderSize;

            //move the pointer
            Vector3 pointerWorldPos = pointerCam.ScreenToWorldPoint(cappedPointerScreenPos);
            pointerRectTransform.position = pointerWorldPos;
            pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);

        }else{
            //pointerImage.sprite = arrivedSprite;
            //if the target is on screen, stay on top of the target
            Vector3 pointerWorldPos = pointerCam.ScreenToWorldPoint(targetPosScreenPoint);
            pointerRectTransform.position = nextNPC.transform.position;
            pointerRectTransform.localEulerAngles = new Vector3(0,0,90);
            pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
        }
    }
}
