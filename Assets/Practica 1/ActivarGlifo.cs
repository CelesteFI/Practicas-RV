using UnityEngine;

public class ActivarGlifo : MonoBehaviour
{
    private Renderer glifoRenderer;

    void Start()
    {
        // Busca el componente internamente al iniciar
        glifoRenderer = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider otro)
    {
        // Se enciende al entrar al radio
        if (otro.CompareTag("Player") && glifoRenderer != null)
        {
            glifoRenderer.material.SetFloat("_Intensity", 2.5f);
        }
    }

    void OnTriggerExit(Collider otro)
    {
        // Se apaga al salir del radio
        if (otro.CompareTag("Player") && glifoRenderer != null)
        {
            glifoRenderer.material.SetFloat("_Intensity", 0f);
        }
    }
}