using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    public GameObject playerObject;
    BirdController playerControls;
    float playerPreviousAccuracy;
    Transform playerTransform;
    Transform myTransform;
    TargetManager targetManager;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.Find("Flyer");
        playerControls = playerObject.GetComponent<BirdController>();
        targetManager = GameObject.Find("_gm").GetComponent<TargetManager>();
    }

    // Update is called once per frame
    void Update()
    {
        myTransform = transform;
        playerTransform = playerObject.transform;
        
        //checkForCollision();
        
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            float distanceToBird = Vector3.Distance(myTransform.position, playerTransform.position);
            

                if (distanceToBird <= playerControls.currentAccuracy)
                {
                    Debug.Log("increase difficulty");
                    targetManager.IncreaseDifficulty();
                    playerControls.currentAccuracy = distanceToBird;
                }
                else
                {
                    Debug.Log("decrease difficulty because of" + playerControls.currentAccuracy);
                    targetManager.decreaseDifficulty();
                    playerControls.currentAccuracy = distanceToBird;
                }

                Destroy(gameObject);

                Debug.Log("collided with size " + transform.localScale.x + " at" + distanceToBird);
            
        }
    }
    void checkForCollision()
    {
        float distanceToBird = Vector3.Distance(myTransform.position, playerTransform.position);
        if (distanceToBird < transform.localScale.x)
        {
            if (Mathf.Abs(playerTransform.position.z - transform.position.z) < 1f)
            {
                
                if(distanceToBird <= playerControls.currentAccuracy)
                {
                    Debug.Log("increase difficulty");
                    targetManager.IncreaseDifficulty();
                    playerControls.currentAccuracy = distanceToBird;
                }
                else
                {
                    Debug.Log("decrease difficulty because of"+playerControls.currentAccuracy);
                    targetManager.decreaseDifficulty();
                    playerControls.currentAccuracy = distanceToBird;
                }

                Destroy(gameObject);

                Debug.Log("collided with size "+transform.localScale.x+" at"+ distanceToBird);
            }
        }
    }
}

