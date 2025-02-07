using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Platform;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIManager : MonoBehaviour
{

    private GameObject jugador; // JUGADOR
    public GameObject[] enemigos; // VARIANTES DE ENEMIGOS, GRANDES Y PEQUEÑOS
    private NavMeshAgent agente; // NAV MESH AGENT
    public float spawnInterval = 10f; // ENEMY SPAWN RATE
    public Transform enemigosParent; // OBJETO PADRE PARA LOS ENEMIGOS GENERADOS
    public Transform[] spawnPoints; // ARRAY DE SPAWN POINTS


    // Start is called before the first frame update
    void Start()
    {
        // BUSCA A CHOMP Y A LOS ENEMIGOS
        jugador = GameObject.Find("Chomp");

        // INICIA LA GENERACIÓN DE ENEMIGOS
        StartCoroutine(GenerarEnemigos());
    }

    // CORUTINA PARA GENERAR ENEMIGOS
    IEnumerator GenerarEnemigos()
    {
        while (true)
        {
            // Espera el tiempo de spawn
            yield return new WaitForSeconds(spawnInterval);

            if (enemigos.Length == 0) yield break; // Evita errores si no hay enemigos

            // Selecciona dos enemigos aleatorios de la lista
            int index1 = Random.Range(0, enemigos.Length);
            int index2 = Random.Range(0, enemigos.Length);

            GameObject enemigo1 = enemigos[index1];
            GameObject enemigo2 = enemigos[index2];

            // Selecciona dos SpawnPoints aleatorios
            int spawnIndex1 = Random.Range(0, spawnPoints.Length);
            int spawnIndex2 = Random.Range(0, spawnPoints.Length);

            // CLONA Y ACTIVA EL PRIMER ENEMIGO
            GameObject clon1 = Instantiate(enemigo1, spawnPoints[spawnIndex1].position, Quaternion.identity);
            clon1.name = enemigo1.name; // Mantiene el mismo nombre
            clon1.SetActive(true);
            clon1.transform.parent = enemigosParent; // Lo coloca bajo el objeto padre
            enemigos = AgregarEnemigoALista(enemigos, clon1);  // Agregar el clon a la lista

            // CLONA Y ACTIVA EL SEGUNDO ENEMIGO - (DESACTIVADO PARA NO AHOGAR AL JUGADOR CON ENEMIGOS)
            //GameObject clon2 = Instantiate(enemigo2, spawnPoints[spawnIndex2].position, Quaternion.identity);
            //clon2.name = enemigo2.name;
            //clon2.SetActive(true);
            //clon2.transform.parent = enemigosParent;
            //enemigos = AgregarEnemigoALista(enemigos, clon2);  // Agregar el clon a la lista
        }
    }

    GameObject[] AgregarEnemigoALista(GameObject[] listaOriginal, GameObject nuevoEnemigo)
    {
        List<GameObject> listaTemp = new List<GameObject>(listaOriginal);
        listaTemp.Add(nuevoEnemigo);
        return listaTemp.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        // ASIGNA LA IA CORRESPONDIENTE SEGÚN EL TIPO DE ENEMIGO
        foreach (GameObject enemigo in enemigos)
        {
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


    // AI BLINKY - SIGUE A CHOMPMAN DIRECTAMENTE
    void BlinkyAI(GameObject enemigo)
    {
        if (!enemigo.activeInHierarchy) return;

        NavMeshAgent agente = enemigo.GetComponent<NavMeshAgent>();

        if (agente != null)
        {
            agente.SetDestination(jugador.transform.position);
        }
    }


    // AI PINKY - INTENTA ADELANTARSE A CHOMPMAN
    void PinkyAI(GameObject enemigo)
    {
        if (!enemigo.activeInHierarchy) return;

        NavMeshAgent agente = enemigo.GetComponent<NavMeshAgent>();

        if (agente != null)
        {
            Vector3 objetivo = jugador.transform.position + (jugador.transform.forward * 4f);
            agente.SetDestination(objetivo);  // Pinky intenta adelantarse a Pac-Man
        }
    }

    // AI INKY - SI BLINKY EXISTE, SE MUEVE ERRÁTICAMENTE BASADA EN LA POSICIÓN DEL PRIMERO
    // SI BLINKY NO EXISTE, PERSIGUE A CHOMPMAN COMO LO HACE BLINKY
    void InkyAI(GameObject enemigo)
    {
        if (!enemigo.activeInHierarchy) return;

        NavMeshAgent agente = enemigo.GetComponent<NavMeshAgent>();

        GameObject blinky = GameObject.Find("Blinky");

        if (agente != null)
        {
            if (blinky != null)
            {
                Vector3 puntoReferencia = jugador.transform.position + (jugador.transform.forward * 2);
                Vector3 objetivo = puntoReferencia + (puntoReferencia - blinky.transform.position);
                agente.SetDestination(objetivo);
            }
            else
            {
                agente.SetDestination(jugador.transform.position);
            }
        }
    }

    // CLYDE - PERSIGUE A CHOMPMAN A NO SER QUE SE ACERQUE, HACIENDO QUE ESTE VUELVA A LA CASILLA DE ESCONDITE
    void ClydeAI(GameObject enemigo)
    {
        if (!enemigo.activeInHierarchy) return;

        NavMeshAgent agente = enemigo.GetComponent<NavMeshAgent>();

        if (agente != null)
        {
            float distancia = Vector3.Distance(enemigo.transform.position, jugador.transform.position);

            if (distancia > 5f)
            {
                agente.SetDestination(jugador.transform.position);  // Clyde persigue
            }
            else
            {
                Vector3 safeZone = new Vector3(0f, enemigo.transform.position.y, 0f);
                agente.SetDestination(safeZone);  // Clyde huye si está cerca
            }
        }
    }
}