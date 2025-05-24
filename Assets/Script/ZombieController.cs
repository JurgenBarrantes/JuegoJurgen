using UnityEngine;

public class MaleZombieController : MonoBehaviour
{
    public int puntosVida = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


    }
    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        puntosVida -= damage;
        Debug.Log($"Zombie recibió {damage} de daño. Vida restante: {puntosVida}");
        if (puntosVida <= 0)
        {
            Debug.Log("Zombie eliminado");
            Destroy(gameObject);
        }
    }
}