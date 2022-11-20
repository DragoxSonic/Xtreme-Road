using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public int carril;
    public int lateral;

    int posicionZ;
    public Vector3 posObjetivo;
    public float velocidad;
    public Mundo mundo;
    public Transform grafico;
    public LayerMask capaObstaculos;
    public float distanciaVista = 1;
    public bool vivo = true;


   
    void Update()
    {
        Actualizarposicion();

        if (Input.GetKeyDown(KeyCode.W))
        {
            Avanzar();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Retroceder();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoverLados(1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            MoverLados(-1);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(grafico.position + Vector3.up * 0.5f,grafico.position + Vector3.up* 0.5f + grafico.forward * distanciaVista);
    }
    public void Actualizarposicion()
    {
        if (!vivo)
        {
            return;
        }
        posObjetivo = new Vector3(lateral, 0, posicionZ);
        transform.position = Vector3.Lerp(transform.position, posObjetivo, velocidad * Time.deltaTime);
    }
    public void Avanzar()
    {
        if (!vivo)
        {
            return;
        }
        grafico.eulerAngles = Vector3.zero;
        if (MirarAdelante())
        {

            return;
        }

        posicionZ++;
        if (posicionZ > carril)
        {
            carril = posicionZ;
            mundo.CrearPisos();
        }
    }
    public void Retroceder()
    {
        if (!vivo)
        {
            return;
        }
        grafico.eulerAngles = new Vector3(0, 180, 0);
        if (MirarAdelante())
        {
            return;
        }

        if (posicionZ > carril - 2)
        {
            posicionZ--;
        }
    }
    public void MoverLados(int cuanto)
    {
        if (!vivo)
        {
            return;
        }
        grafico.eulerAngles = new Vector3(0, 90 * cuanto, 0);
        if (MirarAdelante())
        {
            return;
        }

        lateral += cuanto;
        lateral = Mathf.Clamp(lateral, -4, 4);
    }

    public bool MirarAdelante()
    {
        RaycastHit hit;
        Ray rayo = new Ray(grafico.position + Vector3.up * 0.5f, grafico.forward);

        if (Physics.Raycast(rayo, out hit, distanciaVista, capaObstaculos))
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Carro"))
        {
            vivo = false;
        }
        if (other.CompareTag("Agua"))
        {
            vivo = false;
        }
    }

}