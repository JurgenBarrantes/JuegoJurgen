using UnityEngine;
using UnityEngine.UI;

public class GameManagerController : MonoBehaviour
{
    private GameRepository gameRepository;
    private GameData gameData;
    private Text enemigosMuertosText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameRepository = GameRepository.GetInstance();
        enemigosMuertosText = GameObject.Find("enemigosMuertosText").GetComponent<Text>();
        gameData = gameRepository.GetData();
    }

    // Update is called once per frame


    void Update()
    {
        enemigosMuertosText.text = $"ENEMIGOS: {gameData.enemigosMuertos}";
        
    }
}
