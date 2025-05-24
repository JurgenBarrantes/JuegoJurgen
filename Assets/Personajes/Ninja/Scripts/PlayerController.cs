using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Variables
    public AudioClip disparoClip;
    public AudioClip saltoClip;

    public float velocidad = 10f;
    public float fuerzaSalto = 12.5f;

    public GameObject kunaiPrefab;
    public int kunaisDisponibles = 10;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public AudioSource audioSource;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;


    private bool isGrounded = true;
    private String direccion = "Derecha";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool puedeMoverseVerticalMente = false;
    private float defaultGravityScale = 1f;
    private bool puedeSaltar = true;
    private bool puedeLanzarKunai = true;

    [Header("Parámetros de salto")]
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Coyote Time")]
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("Jump Buffer")]
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    private Text enemigosMuertosText;
    private float kunaiPressStartTime = 0f;
    private bool isPressingKunai = false;

    void Start()
    {
        Debug.Log("INICIANDO PLAYER CONTROLLER");

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

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
            Debug.Log($"Colision con Enemigo: ${zombie.puntosVida}");
            Destroy(collision.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log($"Triggera con: {other.gameObject.name}");
        if (other.gameObject.name == "Muro")
        {
            puedeMoverseVerticalMente = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"Trigger con: {other.gameObject.name}");
        if (other.gameObject.name == "Muro")
        {
            puedeMoverseVerticalMente = false;
            rb.gravityScale = defaultGravityScale;
        }
    }

    void SetupMoverseHorizontal()
    {
        if (isGrounded && rb.linearVelocityY == 0)
        {
            animator.SetInteger("Estado", 0);
        }

        rb.linearVelocityX = 0;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.linearVelocityX = velocidad;
            sr.flipX = false;
            direccion = "Derecha";
            if (isGrounded && rb.linearVelocityY == 0)
                animator.SetInteger("Estado", 1);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.linearVelocityX = -velocidad;
            sr.flipX = true;
            direccion = "Izquierda";
            if (isGrounded && rb.linearVelocityY == 0)
                animator.SetInteger("Estado", 1);
        }

    }
    void SetupMoverVertical()
    {
        if (!puedeMoverseVerticalMente) return;

        rb.gravityScale = 0;
        rb.linearVelocityY = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.linearVelocityY = velocidad;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.linearVelocityY = -velocidad;
        }

    }
    void SetupSalto()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // ---Coyote Time ---
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            if (rb.linearVelocityY > 5f)
                animator.SetInteger("Estado", 3);

        }

        //--- Jump Buffer ---
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        //---Ejecutar salto ---
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocityY = jumpForce;
            jumpBufferCounter = 0f;
        }

        // ---Ajuste de gravedad para caida más rápida ---
        if (rb.linearVelocityY < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1f) * Time.deltaTime;
        }
        else if (rb.linearVelocityY > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;
        }
    }

    void SetUpLanzarKunai()
    {
        if (!puedeLanzarKunai || kunaisDisponibles <= 0) return;
        if (Input.GetKeyDown(KeyCode.K))
        {
            kunaiPressStartTime = Time.time;
            isPressingKunai = true;
        }

        if (Input.GetKeyUp(KeyCode.K) && isPressingKunai)
        {
            float presDuration = Time.time - kunaiPressStartTime;
            int damage = 1;
            Vector3 scale = Vector3.one;

            if (presDuration >= 5f)
            {
                damage = 3;
                scale = new Vector3(4f, 4f, 4f);
            }
            else if (presDuration >= 2f)
            {
                damage = 2;
                scale = new Vector3(2f, 2f, 2f);
            }


            GameObject kunai = Instantiate(kunaiPrefab, transform.position, Quaternion.Euler(0, 0, -90));
            kunai.transform.localScale = scale;
            //kunai.GetComponent<KunaiController>().SetDirection(direccion);
            KunaiController controller = kunai.GetComponent<KunaiController>();
            controller.SetDirection(direccion);
            controller.SetDamage(damage);
            kunaisDisponibles --;
            //ejecutar sonido
            audioSource.PlayOneShot(disparoClip);
            isPressingKunai = false;
        }
    }

    //Visualiza el groundCheck en el editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
        }
    }    
}
