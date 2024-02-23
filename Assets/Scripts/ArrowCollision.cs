using UnityEngine;

public class ArrowCollision : MonoBehaviour
{
    public AudioClip impactSound;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            AudioSource soundManager = FindObjectOfType<AudioSource>();
            if (soundManager != null && impactSound != null)
            {
                soundManager.PlayOneShot(impactSound);
            }
        }

        // Aquí puedes agregar cualquier otro código o lógica que desees para el impacto de las flechas.
    }
}
