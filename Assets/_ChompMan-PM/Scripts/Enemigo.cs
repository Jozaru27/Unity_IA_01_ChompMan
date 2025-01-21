using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour
{
    GameObject Jugador;
    NavMeshAgent Agente;

    // Start is called before the first frame update
    void Start()
    {
        Jugador = GameObject.Find("Chomp");
        Agente =  this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Agente.destination = Jugador.transform.position;
        //Agente.SetDestination(Jugador.transform.position);
    }
}
