using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Variables
    public GameObject kunaiPrefab;
    public int kunaisDisponibles = 5;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    

    private String direccion = "Derecha";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool puedeMoverseVerticalMente = false;
    private float defaultGravityScale = 1f;
    private bool puedeSaltar = true;
    private bool puedeLanzarKunai = true;

    public int Vidas;
    public Text vidasText;
    void Start()
    {
        Debug.Log("INICIANDO PLAYER CONTROLLER");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Vidas = 2;
        vidasText = GameObject.Find("VidasText").GetComponent<Text>();
        vidasText.text= "VIDAS: "+ Vidas;

        defaultGravityScale = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        
        SetupMoverseHorizontal();
        SetupMoverVertical();
        SetupSalto();
        SetUpLanzarKunai();
    }


    //coliciones sem05
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
           
            
            MaleZombieController zombie = collision.gameObject.GetComponent<MaleZombieController>();
            Debug.Log($"Colision con el enemigo: {zombie.puntosVida}");
             Vidas--;
             vidasText.text = "VIDAS: " + Vidas;
            Destroy(collision.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log($"Triggera con: {other.gameObject.name}");
        if(other.gameObject.name == "Muro"){
            puedeMoverseVerticalMente= true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"Trigger con: {other.gameObject.name}");
        if (other.gameObject.name == "Muro") {
            puedeMoverseVerticalMente = false;
            rb.gravityScale = defaultGravityScale;
        }
    }

    void SetupMoverseHorizontal()
    {
        rb.linearVelocityX = 0;
        animator.SetInteger("Estado", 0);

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.linearVelocityX = 10;
            sr.flipX = false;
            direccion ="Derecha";
            animator.SetInteger("Estado", 1);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.linearVelocityX = -10;
            sr.flipX = true;
            direccion ="Izquierda";
            animator.SetInteger("Estado", 1);
        }

    }
    void SetupMoverVertical()
    {
        if(!puedeMoverseVerticalMente) return;
        
        rb.gravityScale = 0;
        rb.linearVelocityY = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.linearVelocityY = 10;
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
                rb.linearVelocityY = -10;
        }
        
    }
    void SetupSalto()
    {
        if (!puedeSaltar) return;
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.linearVelocityY = 12.5f;
        }
    }

    void SetUpLanzarKunai() {
        if (!puedeLanzarKunai || kunaisDisponibles <= 0) return;
        if(Input.GetKeyUp(KeyCode.K))
        {
            GameObject kunai = Instantiate(kunaiPrefab,transform.position, Quaternion.Euler(0, 0, -90));
            kunai.GetComponent<KunaiController>().SetDirection(direccion);
            kunaisDisponibles -=1;
        }
    }

    
    
}
