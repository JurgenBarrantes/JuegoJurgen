using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool puedeMoverseVerticalMente = false;
    private float defaultGravityScale = 1f;
    private bool puedeSaltar = true;
    void Start()
    {
        Debug.Log("INICIANDO PLAYER CONTROLLER");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        defaultGravityScale = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        
        SetupMoverseHorizontal();
        SetupSalto();
        SetupMoverVertical();
    }

    //coliciones sem05
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo")){
            MaleZombieController zombie = collision.gameObject.GetComponent<MaleZombieController>();
            Debug.Log($"Colision con el enemigo: {zombie.puntosVida}");
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
            animator.SetInteger("Estado", 1);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.linearVelocityX = -10;
            sr.flipX = true;
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

    
}
