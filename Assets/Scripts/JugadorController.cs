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

    public bool pelletActivo = false;  // Controla si el pellet está activo

    public bool PelletActivo
    {
        get { return pelletActivo; }
    }

    private float tiempoPelletActivo = 0f;  // Tiempo que el pellet estará activo
    private float velocidadOriginal = 0f;  // Velocidad original de los enemigos
    private float tiempoDuracionPellet = 10f;  // Duración de los efectos del pellet (10 segundos)

    private CanvasIngameManager canvasIngameManager;
    public int DotsContador = 0; 

    void Start()
    {
        controller = this.GetComponent<CharacterController>();
        velocidad = 3f;

        // Encuentra el script CanvasIngameManager en el canvas
        canvasIngameManager = FindObjectOfType<CanvasIngameManager>();
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

        // Colisión con ENEMY - Si el Power Pellet está activo, desactivar al enemigo
        if (other.CompareTag("Enemy"))
        {
            if (pelletActivo)  // Si el Pellet está activo
            {
                other.gameObject.SetActive(false);  // Desactivar al enemigo
                canvasIngameManager.IncrementarContador();
            }
            else
            {
                MenuManager menuManager = FindObjectOfType<MenuManager>();
                menuManager.GameOver(false);  // Si el Pellet no está activo, sigue con el Game Over
            }
        }


        // Colisión con CHERRY - Elimina a los EnemigosActivos. Desaparece
        if (other.CompareTag("Cherry"))
        {
            GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemigo in enemigos)
            {
                enemigo.SetActive(false);  
                canvasIngameManager.IncrementarContador();
            }

            Destroy(other.gameObject);
        }


        // Colisión con POWER PELLET - Desactiva el Pellet, Activa los efectos del Pellet, añade 10 segundos al tiempo.
        if (other.CompareTag("PowerPellet"))
        {
            other.gameObject.SetActive(false);

            ActivarPowerPellet();
        }

        if (other.CompareTag("Dot"))
        {
            // Solo desactivar y contar si el Dot no ha sido recogido antes
            if (other.gameObject.activeSelf)
            {
                // Desactiva el Dot
                other.gameObject.SetActive(false);

                // Incrementa el contador de Dots recogidos
                DotsContador++;

                // Llamar a la función de actualización del contador en el CanvasIngameManager
                canvasIngameManager.ActualizarContadorDots(DotsContador);

                // Verifica si el jugador ha recogido todos los Dots
                VerificarVictoria();
            }
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
    
    void VerificarVictoria()
    {
        // Encuentra todos los Dots en la escena
        GameObject[] dots = GameObject.FindGameObjectsWithTag("Dot");

        // Contar cuántos Dots están desactivados
        int dotsDesactivados = 0;

        foreach (GameObject dot in dots)
        {
            if (!dot.activeSelf)  // Si el Dot está desactivado
            {
                dotsDesactivados++;
            }
        }

        // Si todos los Dots están desactivados, ha ganado
        if (dotsDesactivados >= dots.Length)
        {
            // Llamar a GameOver o la lógica de victoria
            MenuManager menuManager = FindObjectOfType<MenuManager>();
            menuManager.GameOver(true);  // O cambiarlo a un método de victoria si lo prefieres
        }
    }
}