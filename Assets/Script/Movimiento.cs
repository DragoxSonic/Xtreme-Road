using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public int carril;
    public int lateral;
    public Vector3 posObjetivo;
    public float velocidad;
    public Mundo mundo;
    public Transform grafico;
    public LayerMask capaObstaculos;
    public LayerMask capaAgua;
    public float distanciaVista = 1;
    public bool vivo = true;
    public Animator animaciones;
    public AnimationCurve curva;

    bool bloqueo = false;

    int posicionZ;

    private void Start()
    {
        InvokeRepeating("MirarAgua", 1, 0.5f);
    }
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
    }

    public IEnumerator cambiarPosicion()
    {
        bloqueo = true;
        posObjetivo = new Vector3(lateral, 0, posicionZ);
        Vector3 posActual = transform.position;

        for (int i = 0; i < 10; i++)
        {
            transform.position = Vector3.Lerp(posActual, posObjetivo, i * 0.1f) + Vector3.up * curva.Evaluate(i * 0.1f);
            yield return new WaitForSeconds(1f / velocidad);
        }
        bloqueo = false;
    }
    public void Avanzar()
    {
        if (!vivo || bloqueo)
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
        StartCoroutine(cambiarPosicion());
    }
    public void Retroceder()
    {
        if (!vivo || bloqueo)
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
        StartCoroutine(cambiarPosicion());
    }
    public void MoverLados(int cuanto)
    {
        if (!vivo || bloqueo)
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
        StartCoroutine(cambiarPosicion());
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
            animaciones.SetTrigger("Morir");

            vivo = false;
        }
    }

    public void MirarAgua()
    {
        RaycastHit hit;
        Ray rayo = new Ray(grafico.position + Vector3.up, Vector3.down);

        if (Physics.Raycast(rayo, out hit, 3, capaAgua))
        {
            if (hit.collider.CompareTag("Agua"))
            {
                animaciones.SetTrigger("Agua");
                vivo = false;
            }
        }
    }
}
