using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Oyuncu : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float turnSpeed;
    [SerializeField] private Text zaman;
    [SerializeField] private Text text_bitti;
    [SerializeField] private Image kalp;
    [SerializeField] private Button yeniden, cikis;
    [SerializeField] private AudioMixer audioMixer;   

    [SerializeField] private GameObject gameover;
    [SerializeField] private GameObject oyuncu;
    [SerializeField] private Slider sesSlider;
    [SerializeField] private Button sesAc;
    [SerializeField] private Button sesKapa;
     


    float zamanSayaci=100;
    private Vector3 moveDirection;
    private Vector3 velocity;
    public float delay;
    public GameObject patlama;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private GameObject plane;


    private CharacterController controller;
    private Animator anim;
    private void Start()
    {
        sesSlider.value = PlayerPrefs.GetFloat("muzik");
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        gameover.SetActive(false);
        oyuncu.SetActive(true);
    }
    public void Ses(float sesDegeri)
    {
        audioMixer.SetFloat("muzik", Mathf.Log10(sesDegeri) * 20);
        PlayerPrefs.SetFloat("muzik", sesDegeri);
    }
    public void SesAc(float sesDegeri)
    {
        sesSlider.value = 10;
    }
    public void SesKapa(float sesDegeri)
    {
        sesSlider.value = 0;
    }

    public void YenidenOyna()
    {
        gameover.SetActive(false);
        oyuncu.SetActive(true);
        zamanSayaci = 100;
        SceneManager.LoadSceneAsync("SampleScene");
    }
    public void CikisYap()
    {
        Application.Quit();
    }
    private void Update()
    {
        zamanSayaci -= Time.deltaTime;
        zaman.text = (int)zamanSayaci + "";

        float horizontalInput = Input.GetAxis("Horizontal");
        Move();
        if (Input.GetKey(KeyCode.Mouse0))
        {
            StartCoroutine(Attack());
        }
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);
        if (zaman.text == "0")
        {
            SesManagerScript.PlaySound("fail");
            oyuncu.SetActive(false);
            gameover.SetActive(true);
            text_bitti.text = "Oyun Başarısız!!!";
            
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "para")
        {
            Destroy(collision.gameObject);
            zamanSayaci += 10;
            SesManagerScript.PlaySound("coin");
        }
        if (collision.gameObject.tag == "coin")
        {
            Destroy(collision.gameObject);
            SesManagerScript.PlaySound("coin");
        }
        if (collision.gameObject.tag == "elmas")
        {
            Destroy(collision.gameObject);
            zamanSayaci += 10;
            SesManagerScript.PlaySound("coin");
        }
        if (collision.gameObject.tag == "bitti")
        {           
            SesManagerScript.PlaySound("success");
            gameover.SetActive(true);
            oyuncu.SetActive(false);
            text_bitti.text = "Kazandın, Yupppyy :) ";
            SesManagerScript.PlaySound("coin");
        }
        if(collision.gameObject.tag == "bomba")
        {
            Destroy(collision.gameObject);
            SesManagerScript.PlaySound("fail");
            GameObject ptlma = Instantiate(patlama, transform.position, transform.rotation);
            Destroy(ptlma, 3);

            oyuncu.SetActive(false);
            gameover.SetActive(true);
            text_bitti.text = "Bombaya Dokunma Dostum :( ";
        }
        if (collision.gameObject.tag == "zehir")
        {
            Destroy(collision.gameObject);
            SesManagerScript.PlaySound("jump");
            zamanSayaci -= 20;
        }


    }
    
    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float moveZ = Input.GetAxis("Vertical");
        moveDirection = new Vector3(0, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);

        if (isGrounded)
        {
            if (moveDirection!=Vector3.zero && !Input.GetKey(KeyCode.RightShift))
            {
                Walk();
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.RightShift))
            {
                Run();
            }
            else if(moveDirection==Vector3.zero)
            {
                Idle();
            }
            moveDirection *= moveSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        
       
        controller.Move(moveDirection * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void Idle()
    {
        anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
         
    }
    private void Walk()
    {
        moveSpeed = walkSpeed;
        anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }
    private void Run()
    {
        moveSpeed = runSpeed;
        anim.SetFloat("Speed", 1,0.1f,Time.deltaTime);
    }
    private void Jump()
    {

        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        SesManagerScript.PlaySound("jump");

    }
    private IEnumerator Attack()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 1);
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.9f);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 0);
    }
}
