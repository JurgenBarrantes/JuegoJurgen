using UnityEngine;
using UnityEngine.SceneManagement;
public class GameMenuController : MonoBehaviour
{
    GameObject personaje1;
    GameObject personaje2;
    GameRepository gameRepository;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        personaje1 = GameObject.Find("Personaje1");
        personaje2 = GameObject.Find("Personaje2");
        personaje1.SetActive(true);
        personaje2.SetActive(false);

        gameRepository = GameRepository.GetInstance();

        gameRepository.GetData().personaje = "personaje1";
        gameRepository.SaveData();
    }

    public void IniciarJuego()
    {
        Debug.Log("Iniciar Juego");

        //Iniciar la escena del juego
        SceneManager.LoadScene("SampleScene");

    }

    public void SeleccionarPersonaje()
    {
        personaje1.SetActive(!personaje1.activeSelf);
        personaje2.SetActive(!personaje2.activeSelf);

        if (personaje1.activeSelf)
        {
            gameRepository.GetData().personaje = "personaje1";
        }
        else
        {
            gameRepository.GetData().personaje = "personaje2";
        }

        gameRepository.SaveData();

        //Iniciar la escena del juego 

    }
}
