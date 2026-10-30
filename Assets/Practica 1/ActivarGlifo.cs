using UnityEngine;

public class ActivarGlifo : MonoBehaviour
{
    private Renderer glifoRenderer;

    void Start()
    {
        // Busca el componente internamente al iniciar, evitando la serialización pública
        glifoRenderer = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider otro)
    {
        if (otro.CompareTag("Player") && glifoRenderer != null)
        {
            glifoRenderer.material.SetFloat("_Intensity", 2.5f);
        }
    }
}