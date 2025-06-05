using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManagerController : MonoBehaviour
{
    private GameRepository gameRepository;
    private GameData gameData;
    GameObject personajeLocation;
    public List<GameObject> personajes = new List<GameObject>();
    private Text enemigosMuertosText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameRepository = GameRepository.GetInstance();
        personajeLocation = GameObject.Find("PersonajeLocation");
        enemigosMuertosText = GameObject.Find("enemigosMuertosText").GetComponent<Text>();
        gameData = gameRepository.GetData();

        //Inicializar el oersonaje seleccionado
        InsertarPersonajeEnEscena();
    }

    // Update is called once per frame


    void Update()
    {
        enemigosMuertosText.text = $"ENEMIGOS: {gameData.enemigosMuertos}";

    }

    void InsertarPersonajeEnEscena()
    {
        var gameObject = gameData.personaje == "personaje1" ? personajes[0] : personajes[1];
        Instantiate(gameObject, personajeLocation.transform.position, personajeLocation.transform.rotation, personajeLocation.transform);
        
    }
}
