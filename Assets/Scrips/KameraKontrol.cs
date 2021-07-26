using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraKontrol : MonoBehaviour
{
    [SerializeField]

    GameObject player;
    Vector3 aradakifark;


     
    void Start()
    {
        aradakifark = transform.position - player.transform.position;

        //aradaki farkı buluyoruz
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + aradakifark;

    }
}
   
