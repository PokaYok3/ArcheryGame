using UnityEngine;

public class TargetCollisionDetector : MonoBehaviour
{
    private TargetSpawner targetSpawner;
    private bool isSpecialTarget = false;
    public void Initialize(TargetSpawner spawner) //Used to initialize the targetCollisionDetector
    {
        targetSpawner = spawner;
    }
    public void SetSpecialTarget()
    {
        isSpecialTarget = true;
    }
    public void SetNormalTarget()
    {
        isSpecialTarget = false;
    }
    
    private void OnCollisionEnter(Collision collision)// Actions to be done in case of a collision
    {
        // The sound is played when the collision is done by an arrow
        if (collision.gameObject.CompareTag("Pickable")) // To identify the arrow
        {
            if (isSpecialTarget==true)
            {
                AudioSource.PlayClipAtPoint(targetSpawner.specialHit, transform.position); //Reproduction of the sound
                targetSpawner.increaseCollisionCount();
                foreach (GameObject target in targetSpawner.spawnedTargets)
                {
                    Destroy(target);
                }
                targetSpawner.spawnedTargets.Clear(); // Clean the list of targets
                Destroy(gameObject); //Destroy the target

                
            }
            if(isSpecialTarget==false){
                AudioSource.PlayClipAtPoint(targetSpawner.collisionSound, transform.position); //Reproduction of the sound
                targetSpawner.decreaseCollisionCount();
                foreach (GameObject target in targetSpawner.spawnedTargets)
                {
                    Destroy(target);
                }
                targetSpawner.spawnedTargets.Clear(); // Clean the list of targets
                Destroy(gameObject); //Destroy the target
            }
            
            
        }
    }
    
}
