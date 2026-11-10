using UnityEngine;
using UnityEngine.AI;

public class SeguirJugador : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;
    public Animator anim;

    [Header("Configuración de Ataque")]
    public float distanciaAtaque = 1.8f;

    private NavMeshAgent agente;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // 1. REVISAR SI EL JUEGO YA TERMINÓ
        if (ActivarGlifo.jugadorA_Salvo)
        {
            agente.isStopped = true; // Frena en seco al NavMesh

            if (anim != null)
            {
                anim.SetFloat("Velocidad", 0f); // Regresa a Idle
                anim.SetBool("Near", false);    // Apaga el loop de golpes
            }
            return; // Corta la ejecución aquí; ignora todo lo de abajo para siempre.
        }

        // 2. COMPORTAMIENTO NORMAL (Si el jugador aún no llega al centro)
        if (jugador != null)
        {
            agente.isStopped = false; // Aseguramos que pueda moverse
            agente.SetDestination(jugador.position);

            float distancia = Vector3.Distance(transform.position, jugador.position);

            if (anim != null)
            {
                anim.SetFloat("Velocidad", agente.velocity.magnitude);

                if (distancia <= distanciaAtaque)
                {
                    anim.SetBool("Near", true);
                }
                else
                {
                    anim.SetBool("Near", false);
                }
            }
        }
    }
}