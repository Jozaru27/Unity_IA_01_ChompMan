using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Paneles de los distintos menús
    public GameObject PanelPrincipal; // Panel con la imagen de fondo
    public GameObject PanelMenú;    // Panel del Menú Principal
    public GameObject PanelGameOver;    // Panel de Game Over

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        PanelPrincipal.SetActive(true);
        PanelMenú.SetActive(true);
        PanelGameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jugar()
    {
        Time.timeScale = 1f;  // Reanudar el juego
        PanelPrincipal.SetActive(false);
        PanelMenú.SetActive(false);  // Ocultar el menú
        PanelGameOver.SetActive(false);
    }

    public void Puntuaciones()
    {
        Debug.Log("Puntuaciones aún no implementado.");
    }

    public void GameOver()
    {
        Time.timeScale = 0f;  // Pausar el juego
        PanelPrincipal.SetActive(false);
        PanelMenú.SetActive(false);  // Ocultar el menú principal
        PanelGameOver.SetActive(true);  // Mostrar el panel de Game Over
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 0f;  // Restaurar el tiempo
        PanelPrincipal.SetActive(true);
        PanelMenú.SetActive(true);  // Mostrar el menú principal
        PanelGameOver.SetActive(false);  // Ocultar el panel de Game Over

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();  // Cierra el juego
    }
}
