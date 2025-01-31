using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIManager : MonoBehaviour
{

    // JUGADOR
    private GameObject jugador;

    // VARIANTES DE ENEMIGOS, GRANDES Y PEQUEÑOS
    private GameObject[] enemigos;

    // NAVMESHAGENT
    private NavMeshAgent agente;

    // Start is called before the first frame update
    void Start()
    {
        jugador = GameObject.Find("Chomp");

        enemigos = GameObject.FindGameObjectsWithTag("Enemy");

        // Asigna NavMeshAgent a cada enemigo
        foreach (GameObject enemigo in enemigos)
        {
            enemigo.AddComponent<NavMeshAgent>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject enemigo in enemigos)
        {
            // Asignar la IA correspondiente según el nombre del enemigo
            if (enemigo.name == "Blinky" || enemigo.name == "BlinkyBig")
            {
                BlinkyAI(enemigo);
            }
            else if (enemigo.name == "Pinky" || enemigo.name == "PinkyBig")
            {
                PinkyAI(enemigo);
            }
            else if (enemigo.name == "Inky" || enemigo.name == "InkyBig")
            {
                InkyAI(enemigo);
            }
            else if (enemigo.name == "Clyde" || enemigo.name == "ClydeBig")
            {
                ClydeAI(enemigo);
            }
        }
    }

    void BlinkyAI(GameObject enemigo)
    {
        NavMeshAgent agente = enemigo.GetComponent<NavMeshAgent>();
        agente.SetDestination(jugador.transform.position);  // Blinky sigue a Pac-Man directamente
    }

    void PinkyAI(GameObject enemigo)
    {
        NavMeshAgent agente = enemigo.GetComponent<NavMeshAgent>();
        Vector3 objetivo = jugador.transform.position + (jugador.transform.forward * 4f);
        agente.SetDestination(objetivo);  // Pinky intenta adelantarse a Pac-Man
    }

    void InkyAI(GameObject enemigo)
    {
        NavMeshAgent agente = enemigo.GetComponent<NavMeshAgent>();
        GameObject blinky = GameObject.Find("Blinky");  // Blinky solo se usa para Inky

        if (blinky != null)
        {
            Vector3 puntoReferencia = jugador.transform.position + (jugador.transform.forward * 2);
            Vector3 objetivo = puntoReferencia + (puntoReferencia - blinky.transform.position);
            agente.SetDestination(objetivo);  // Inky se mueve erráticamente basado en Blinky
        }
        else
        {
            agente.SetDestination(jugador.transform.position);  // Si Blinky no está, Inky sigue a Pac-Man
        }
    }

    void ClydeAI(GameObject enemigo)
    {
        NavMeshAgent agente = enemigo.GetComponent<NavMeshAgent>();
        float distancia = Vector3.Distance(enemigo.transform.position, jugador.transform.position);

        if (distancia > 8f)
        {
            agente.SetDestination(jugador.transform.position);  // Clyde persigue
        }
        else
        {
            Vector3 esquinaSegura = new Vector3(-10f, enemigo.transform.position.y, -10f);  // Esquina inferior izquierda
            agente.SetDestination(esquinaSegura);  // Clyde huye si está cerca
        }
    }
}
