using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPelletManager : MonoBehaviour
{
    public GameObject[] powerPellets;  // Array de los Power Pellets
    private GameObject pelletActivo;   // Power Pellet Activo

    // Start is called before the first frame update
    void Start()
    {
        // Encuentra todos los Objetos con el Tag PowerPellet
        powerPellets = GameObject.FindGameObjectsWithTag("PowerPellet");

        // Desactiva todos los Power Pellets
        foreach (GameObject pellet in powerPellets)
        {
            pellet.SetActive(false);
        }

        // Corrutina para Activar/Desactivar Power Pellets
        StartCoroutine(ActivarPowerPellet());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator ActivarPowerPellet()
    {
        while (true)
        {
            // Espera 10 Segundos
            yield return new WaitForSeconds(10f);

            // Si existe un Pellet Activo, lo desactiva.
            if (pelletActivo != null)
            {
                pelletActivo.SetActive(false);
            }

            // Selecciona un Pellet random del índice.
            int index = Random.Range(0, powerPellets.Length);
            pelletActivo = powerPellets[index];

            // Activa el Pellet Seleccionado
            pelletActivo.SetActive(true);
        }
    }
}
