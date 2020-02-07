using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathBehavior : MonoBehaviour
{
     AudioSource mySource;
    [SerializeField] AudioClip hit;
    MeshRenderer myMesh;
    // Start is called before the first frame update
    void Start()
    {
        mySource = GetComponent<AudioSource>();
        myMesh = GetComponent<MeshRenderer>();
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

            //Destroy(gameObject);
        }
    }
}
