using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JugadorController : MonoBehaviour
{
    float velocidad;
    GameObject GameobjectwithCharacterController;
    CharacterController controller;

    void Start()
    {
        controller = this.GetComponent<CharacterController>();
        velocidad = 3f;
    }

    void Update()
    {
    }

    void FixedUpdate()
    {

        //Capturo el movimiento en los ejes
        float movimientoH = Input.GetAxis("Horizontal");
        float movimientoV = Input.GetAxis("Vertical");

        Vector3 anguloTeclas = new Vector3(movimientoH, 0f, movimientoV);
        
        // transform.Translate(anguloTeclas * velocidad * Time.deltaTime, Space.World);

        //Genero el vector de movimiento
        //Muevo el jugador
        //transform.position += anguloTeclas * velocidad * Time.deltaTime;

        controller.Move(anguloTeclas * velocidad * Time.deltaTime);
        if (anguloTeclas != null && anguloTeclas != Vector3.zero)
        {
            transform.forward = anguloTeclas * 1;
            transform.rotation = Quaternion.LookRotation(anguloTeclas);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {

        // Si el jugador colide con un enemigo, se muestra "Game Over" en la consola
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Game Over");
            // Aquí puedes activar el menú de Game Over o cualquier otra acción
        }

        // Detecta colisión con la Cherry
        if (other.CompareTag("Cherry"))
        {
            // Encuentra los enemigos por su tag y los desactiva
            GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemigo in enemigos)
            {
                enemigo.SetActive(false);  // Desactiva cada enemigo encontrado
            }

            // Destruye o desactiva la Cherry
            Destroy(other.gameObject);
        }
    }
}