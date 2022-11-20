using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetornoCarros : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Carro"))
        {
            other.transform.Translate(0, 0, -22);
        }
    }
}
