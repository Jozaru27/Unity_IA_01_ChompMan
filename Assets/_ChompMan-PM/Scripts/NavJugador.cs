using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NavJugador : MonoBehaviour
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
        // Detecta colisión con el objeto Cherry
        if (other.CompareTag("Cherry"))
        {
            // Encuentra los enemigos por su nombre y los desactiva
            GameObject blinky = GameObject.Find("Blinky");
            GameObject bigBlinky = GameObject.Find("Big_Blinky");

            if (blinky != null)
            {
                blinky.SetActive(false);
            }

            if (bigBlinky != null)
            {
                bigBlinky.SetActive(false);
            }

            // Destruye o desactiva la Cherry
            Destroy(other.gameObject);
        }
    }

}