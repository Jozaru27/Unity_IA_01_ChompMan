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

    private JugadorController jugadorController;

    private Dictionary<GameObject, Material> materialesOriginales = new Dictionary<GameObject, Material>();
    public Material ScaredGhost_MAT;  // Variable pública para asignar el material en el Inspector

    // Start is called before the first frame update
    void Start()
    {
        // BUSCA A CHOMP Y A LOS ENEMIGOS
        jugador = GameObject.Find("Chomp");

        if (jugador != null)
        {
            jugadorController = jugador.GetComponent<JugadorController>(); // Obtiene JugadorController de Chomp
        }

    foreach (GameObject enemigo in enemigos)
    {
        Renderer renderer = enemigo.GetComponent<Renderer>();
        if (renderer != null)
        {
            materialesOriginales[enemigo] = renderer.sharedMaterial; // Usa sharedMaterial en lugar de material
        }
    }

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

            // Selecciona un enemigo aleatorio de la lista
            int index1 = Random.Range(0, enemigos.Length);

            GameObject enemigo1 = enemigos[index1];

            // Selecciona un SpawnPoint aleatorio
            int spawnIndex1 = Random.Range(0, spawnPoints.Length);

            // Asegúrate de que la posición Y del spawn sea 1
            Vector3 spawnPosition1 = spawnPoints[spawnIndex1].position;
            spawnPosition1.y = 1f;  // Fijamos la altura en Y a 1

            // CLONA Y ACTIVA EL PRIMER ENEMIGO
            GameObject clon1 = Instantiate(enemigo1, spawnPosition1, Quaternion.identity);
            clon1.name = enemigo1.name; // Mantiene el mismo nombre
            clon1.SetActive(true);
            clon1.transform.parent = enemigosParent; // Lo coloca bajo el objeto padre
            enemigos = AgregarEnemigoALista(enemigos, clon1);  // Agregar el clon a la lista

        }
    }

    GameObject[] AgregarEnemigoALista(GameObject[] listaOriginal, GameObject nuevoEnemigo)
    {
        List<GameObject> listaTemp = new List<GameObject>(listaOriginal);
        listaTemp.Add(nuevoEnemigo);
        return listaTemp.ToArray();
    }

    void Update()
    {
        foreach (GameObject enemigo in enemigos)
        {
            if (!enemigo.activeInHierarchy) continue;

            NavMeshAgent agente = enemigo.GetComponent<NavMeshAgent>();
            Renderer renderer = enemigo.GetComponent<Renderer>();

            if (agente == null || renderer == null) continue;

            // Guardar el material original si no se ha guardado previamente
            if (!materialesOriginales.ContainsKey(enemigo))
            {
                materialesOriginales[enemigo] = renderer.sharedMaterial;
            }

            if (jugadorController != null && jugadorController.PelletActivo)
            {
                agente.SetDestination(new Vector3(0f, 0f, 0f)); // Ir a la Safe Zone
                
                if (ScaredGhost_MAT != null)
                {
                    renderer.sharedMaterial = ScaredGhost_MAT; // Cambiar a material azul (ScaredGhost_MAT)
                }
            }
            else
            {
                AplicarIA(enemigo, agente);

                // Restaurar el material original del enemigo
                if (materialesOriginales.ContainsKey(enemigo))
                {
                    renderer.sharedMaterial = materialesOriginales[enemigo]; // Restaurar material original
                }
            }
        }
    }

    void AplicarIA(GameObject enemigo, NavMeshAgent agente)
    {
        if (enemigo.name.Contains("Blinky"))
        {
            agente.SetDestination(jugador.transform.position);
        }
        else if (enemigo.name.Contains("Pinky"))
        {
            Vector3 objetivo = jugador.transform.position + (jugador.transform.forward * 4f);
            agente.SetDestination(objetivo);
        }
        else if (enemigo.name.Contains("Inky"))
        {
            GameObject blinky = GameObject.Find("Blinky");
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
        else if (enemigo.name.Contains("Clyde"))
        {
            float distancia = Vector3.Distance(enemigo.transform.position, jugador.transform.position);
            if (distancia > 5f)
            {
                agente.SetDestination(jugador.transform.position);
            }
            else
            {
                agente.SetDestination(new Vector3(0f, 0f, 0f)); // Safe Zone
            }
        }
    }

}