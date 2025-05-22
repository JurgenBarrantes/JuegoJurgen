using UnityEngine;

public class KunaiController : MonoBehaviour
{
    private string direccion = "Derecha";
    Rigidbody2D rb;
    SpriteRenderer sr;
    GameRepository gameRepository;
    GameData gameData;
    //private static bool puntosIniciados = false;  // Se asegura de iniciar solo una vez

    void Start()
    {
        gameRepository = GameRepository.GetInstance();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        gameData = gameRepository.GetData();

        Destroy(this.gameObject, 5f);
        
    }

    void Update()
    {
        if (direccion == "Derecha")
        {
            rb.linearVelocityX = 15;
            sr.flipY = false;
        }
        else if (direccion == "Izquierda")
        {
            rb.linearVelocityX = -15;
            sr.flipY = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Colision con: {collision.gameObject.tag}");
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            //Puntos++;
            //puntosText.text = "PUNTOS: "+ Puntos;
            Destroy(collision.gameObject);
            Destroy(this.gameObject);

            gameData.enemigosMuertos++;
            gameRepository.SaveData();
            
        }
    }

    public void SetDirection(string direction)
    {
        this.direccion = direction;
    }
}
