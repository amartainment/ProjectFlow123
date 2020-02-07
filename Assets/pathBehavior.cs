using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathBehavior : MonoBehaviour
{
     AudioSource mySource;
    [SerializeField] AudioClip hit;
    MeshRenderer myMesh;
    TargetManager gm;
    // Start is called before the first frame update
    void Start()
    {
        mySource = GetComponent<AudioSource>();
        myMesh = GetComponent<MeshRenderer>();
        gm = GameObject.Find("_gm").GetComponent<TargetManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            mySource.PlayOneShot(hit);
            myMesh.enabled = false;
            gm.increasePathScore();
            //Destroy(gameObject);
        }
    }
}
