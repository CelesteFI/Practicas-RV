using UnityEngine;

public class ActivarGlifo : MonoBehaviour
{
    private Renderer glifoRenderer;

    // Variable global que todos los enemigos pueden leer
    public static bool jugadorA_Salvo = false;

    void Start()
    {
        glifoRenderer = GetComponent<Renderer>();
        // Reiniciamos el estado al darle Play
        jugadorA_Salvo = false;
        // Nos aseguramos de que empiece apagado
        if (glifoRenderer != null) glifoRenderer.material.SetFloat("_Intensity", 0f);
    }

    void OnTriggerEnter(Collider otro)
    {
        if (otro.CompareTag("Player") && glifoRenderer != null)
        {
            // Se enciende y se queda así para siempre
            glifoRenderer.material.SetFloat("_Intensity", 2.5f);

            // ¡Ganaste! Avisamos a los golems para que se apaguen
            jugadorA_Salvo = true;
        }
    }
}