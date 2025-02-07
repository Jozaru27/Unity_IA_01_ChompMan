using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class JugadorController : MonoBehaviour
{
    float velocidad;
    GameObject GameobjectwithCharacterController;
    CharacterController controller;

    private bool pelletActivo = false;  // Controla si el pellet está activo
    private float tiempoPelletActivo = 0f;  // Tiempo que el pellet estará activo
    private float velocidadOriginal = 0f;  // Velocidad original de los enemigos
    private float tiempoDuracionPellet = 10f;  // Duración de los efectos del pellet (10 segundos)


    void Start()
    {
        controller = this.GetComponent<CharacterController>();
        velocidad = 3f;
    }


    void Update()
    {
        // Si el pellet está activo, contamos el tiempo
        if (pelletActivo)
        {
            tiempoPelletActivo -= Time.deltaTime;  // Reducimos el tiempo restante

            if (tiempoPelletActivo <= 0f)
            {
                DesactivarPowerPellet();  // Desactivar el efecto del pellet cuando el tiempo se acabe
            }
        }
    }


    void FixedUpdate()
    {
        // MOVIMIENTO
        float movimientoH = Input.GetAxis("Horizontal");
        float movimientoV = Input.GetAxis("Vertical");

        Vector3 anguloTeclas = new Vector3(movimientoH, 0f, movimientoV);
        
        controller.Move(anguloTeclas * velocidad * Time.deltaTime);
        if (anguloTeclas != null && anguloTeclas != Vector3.zero)
        {
            transform.forward = anguloTeclas * 1;
            transform.rotation = Quaternion.LookRotation(anguloTeclas);
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {


        // Colisión con ENEMY - Game Over
        if (other.CompareTag("Enemy"))
        {
            MenuManager menuManager = FindObjectOfType<MenuManager>();

            menuManager.GameOver(); 
        }


        // Colisión con CHERRY - Elimina a los EnemigosActivos. Desaparece
        if (other.CompareTag("Cherry"))
        {
            GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemigo in enemigos)
            {
                enemigo.SetActive(false);  
            }

            Destroy(other.gameObject);
        }


        // Colisión con POWER PELLET - Desactiva el Pellet, Activa los efectos del Pellet, añade 10 segundos al tiempo.
        if (other.CompareTag("PowerPellet"))
        {
            other.gameObject.SetActive(false);

            ActivarPowerPellet();
        }
    }

    void ActivarPowerPellet()
    {
        pelletActivo = true;
        tiempoPelletActivo = tiempoDuracionPellet;  // Establecer el tiempo activo del pellet a 10 segundos

        // Reducir la velocidad de todos los enemigos
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemigo in enemigos)
        {
            NavMeshAgent agente = enemigo.GetComponent<NavMeshAgent>();
            if (agente != null)
            {
                velocidadOriginal = agente.speed;  // Guardar la velocidad original
                agente.speed *= 0.5f;  // Reducir la velocidad a la mitad
            }
        }
    }

    void DesactivarPowerPellet()
    {
        // Restaurar la velocidad de todos los enemigos
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemigo in enemigos)
        {
            NavMeshAgent agente = enemigo.GetComponent<NavMeshAgent>();
            if (agente != null)
            {
                agente.speed = velocidadOriginal;  // Restaurar la velocidad original
            }
        }

        // Restablecer el estado del pellet
        pelletActivo = false;
        tiempoPelletActivo = 0f;
    }
}