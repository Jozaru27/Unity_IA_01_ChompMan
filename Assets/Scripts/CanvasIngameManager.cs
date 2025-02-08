using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasIngameManager : MonoBehaviour
{

    public TextMeshProUGUI textoEnemigosDestruidos;  // Si usas TextMeshPro

    public TextMeshProUGUI contadorDotsText;  // Asegúrate de asignar este TextMeshProUGUI en el inspector

    private int enemigosDestruidos = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Este método incrementa el contador cuando un enemigo es destruido
    public void IncrementarContador()
    {
        enemigosDestruidos++;
        ActualizarTexto();
    }

    // Actualiza el texto en el Canvas
    private void ActualizarTexto()
    {
        textoEnemigosDestruidos.text = "Enemigos Destruidos: " + enemigosDestruidos.ToString();
    }

    public void ActualizarContadorDots(int nuevoContador)
    {
        if (contadorDotsText != null)
        {
            contadorDotsText.text = "Dots: " + nuevoContador.ToString();  // Actualiza el contador
        }
    }
}
