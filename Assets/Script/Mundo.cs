using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mundo : MonoBehaviour
{
    public int carril = 1;
    public GameObject[] pisos;
    public int pisosDiferencia;

    private void Start()
    {
        for (int i = 1; i < pisosDiferencia; i++)
        {
            CrearPisos();
        }
    }
    public void CrearPisos()
    {
        Instantiate(pisos[Random.Range(0, pisos.Length)], Vector3.forward * carril, Quaternion.identity);
        carril++;
    }
}
