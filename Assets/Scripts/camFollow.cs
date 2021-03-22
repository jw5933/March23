using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollow : MonoBehaviour
{
    public Transform followTransform;
    public BoxCollider2D worldBounds;
    public Renderer player;

    float playerHeight;
    float xMin, xMax, yMin, yMax;
    float camY, camX;

    Camera mainCam;

    float camRatio; //length
    float camSize; //width of cam

    Vector3 smoothPos;

    public float smoothRate;

    // Start is called before the first frame update
    void Start()
    {
        xMin = worldBounds.bounds.min.x;
        xMax = worldBounds.bounds.max.x;
        yMin = worldBounds.bounds.min.y;
        yMax = worldBounds.bounds.max.y;

        mainCam = gameObject.GetComponent<Camera>();

        camSize=mainCam.orthographicSize;
        camRatio = (xMax + camSize)/8.0f;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // camX = Mathf.Clamp(followTransform.position.x, xMin+camRatio, xMax-camRatio);
        // camY = Mathf.Clamp(followTransform.position.y, yMin+camSize, yMax-camSize);
    
        camX = Mathf.Clamp(player.bounds.center.x, xMin+camRatio, xMax-camRatio);
        camY = Mathf.Clamp(player.bounds.center.y, yMin+camSize, yMax-camSize);
        

        //linear interpolation
        smoothPos = Vector3.Lerp(gameObject.transform.position, new Vector3(camX, camY, gameObject.transform.position.z), smoothRate);
        
        gameObject.transform.position = smoothPos;
    }
}
