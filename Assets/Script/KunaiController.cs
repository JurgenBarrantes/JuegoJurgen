using System;
using UnityEngine;
using UnityEngine.UI;

public class KunaiController : MonoBehaviour
{
    private string direccion = "Derecha";
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public static Text puntosText;
    private static int Puntos = 0;
    //private static bool puntosIniciados = false;  // Se asegura de iniciar solo una vez

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        Destroy(this.gameObject, 5f);
        

        // Buscar y asignar el Text si aún no está asignado
        if (puntosText == null)
        {
            GameObject textoGO = GameObject.Find("enemigosMuertosText"); // Asegúrate de que se llame así
            if (textoGO != null)
            {
                puntosText = textoGO.GetComponent<Text>();
                puntosText.text = "KILLS: " + Puntos;
            }
        }
    }

    void Update()
    {
        if (direccion == "Derecha")
        {
            rb.linearVelocity = new Vector2(15, 0);
            sr.flipY = false;
        }
        else if (direccion == "Izquierda")
        {
            rb.linearVelocity = new Vector2(-15, 0);
            sr.flipY = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log($"Colision con: {collision.gameObject.tag}");
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            //Puntos++;
            //puntosText.text = "PUNTOS: "+ Puntos;
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
            
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            Puntos++;

            if (puntosText != null)
            {
                puntosText.text = "KILLS: " + Puntos;
            }

            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void SetDirection(string direction)
    {
        direccion = direction;
    }
}
