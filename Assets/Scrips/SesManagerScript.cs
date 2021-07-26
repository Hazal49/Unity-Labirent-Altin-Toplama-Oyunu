using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SesManagerScript : MonoBehaviour
{
    public static AudioClip coin, fail,jump, success;
    static AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        coin = Resources.Load<AudioClip>("coin");
        fail = Resources.Load<AudioClip>("fail");
        jump = Resources.Load<AudioClip>("jump");
        success = Resources.Load<AudioClip>("success");
        audioSrc = GetComponent<AudioSource>();        
    }
    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "coin":
                audioSrc.PlayOneShot(coin);
                break;
            case "fail":
                audioSrc.PlayOneShot(fail);
                break;
            case "jump":
                audioSrc.PlayOneShot(jump);
                break;
            case "success":
                audioSrc.PlayOneShot(success);
                break;
        }
    }
}