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

        // Colisión con ENEMY
        if (other.CompareTag("Enemy"))
        {
        
            MenuManager menuManager = FindObjectOfType<MenuManager>();

            menuManager.GameOver(); 
        }
        // Colisión con CHERRY
        if (other.CompareTag("Cherry"))
        {
            
            GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemigo in enemigos)
            {
                enemigo.SetActive(false);  
            }

            Destroy(other.gameObject);
        }
    }
}