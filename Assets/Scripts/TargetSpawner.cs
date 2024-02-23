using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class TargetSpawner : MonoBehaviour
{
    //public GameObject[] targetPrefabs; // List of targets to spawn
    public GameObject targetPrefab;
    public Transform spawnZone; // Zone of respawning targets
    public float targetLifetime = 2f; // Time of life of target
    public float timeBetweenTargets = 1f;

    private bool automaticSpawn = false; // Control flag to enable automatic spawning
    private Coroutine spawnCoroutine; // Routine to spawn.
    public AudioClip collisionSound; // Audio clip to play when collided.
    public AudioClip startSound; // Audio clip to play when spawned.
    public AudioClip finishSound; // Audio clip to play when spawned.
    public AudioClip specialHit; // Audio clip to play when hit special target. 
    public AudioClip EnvSound; // Audio clip to play when spawned.
    public int collisionCount = 0;
    public int roundsCount = 0;
    public int TotalRounds = 20;
    public List<GameObject> spawnedTargets = new List<GameObject>();
    public List<Vector3> spawnPositions=new List<Vector3>();

    //public Color normalColor;
    //public Color specialColor;

    public Color[] normalColors;
    public Color[] specialColors;



    private void Start(){
        Vector3 spawnPosition = new Vector3(spawnZone.position.x, spawnZone.position.y, spawnZone.position.z);
        spawnPositions.Add(spawnPosition);
    
        Vector3 spawnPosition2=spawnPosition+2*spawnZone.transform.forward;
        spawnPositions.Add(spawnPosition2);
        Vector3 spawnPosition3=spawnPosition-2*spawnZone.transform.forward;
        spawnPositions.Add(spawnPosition3);
        collisionCount = 0;
        roundsCount = 0;

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))//Start the game
        {
            StartAutomaticSpawn();
        }
        
    }

    private void StartAutomaticSpawn()//Function to start automatic
    {
        if (!automaticSpawn)
        {
            automaticSpawn = true;
            AudioSource.PlayClipAtPoint(startSound, transform.position);

            spawnCoroutine = StartCoroutine(SpawnTargets());
        }
        
    }

    // Control of the automatic spawn
    private IEnumerator SpawnTargets()
    {
        yield return new WaitForSeconds(4f);

        
        while (roundsCount<=TotalRounds) //isautomatic
        {
            float elapsedTime = 0f;
            Quaternion spawnRotation = Quaternion.Euler(90f, -45f, 0f); 
            SpawnTarget(spawnRotation);
            
            Debug.Log("Count of targets: " + spawnedTargets.Count);
            
            while (elapsedTime < targetLifetime && spawnedTargets.Count > 0)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            foreach (GameObject target in spawnedTargets)
            {
                Destroy(target);
            }
            spawnedTargets.Clear();

            yield return new WaitForSeconds(timeBetweenTargets);

            roundsCount++;
        }
        AudioSource.PlayClipAtPoint(finishSound, transform.position);

        // Reset so there are multiple rounds
        automaticSpawn = false;
        roundsCount = 0;
    }

    // Generate a position for the target
    private Vector3 GenerateSpawnPosition()
    {
        Vector3 spawnPosition = new Vector3(spawnZone.position.x, spawnZone.position.y, spawnZone.position.z);
        spawnPosition=spawnPosition+Random.Range(-5f,+5f)*spawnZone.transform.forward;
        return spawnPosition;
    }

    // Execute the spawn
    private void SpawnTarget(Quaternion rotation)
    {
        //Code to generate the positions of the targets
        int randomIndex1 = Random.Range(0, 3); 
        int randomIndex2 = Random.Range(0, 3); 

        while (randomIndex2 == randomIndex1) 
        {
            randomIndex2 = Random.Range(0, 3); 
        }

        int randomIndex3 = Random.Range(0, 3); 

        while (randomIndex3 == randomIndex1 || randomIndex3 == randomIndex2) 
        {
            randomIndex3 = Random.Range(0, 3); 
        }

        Vector3 spawnPosition1 = spawnPositions[randomIndex1];
        Vector3 spawnPosition2 = spawnPositions[randomIndex2];
        Vector3 spawnPosition3 = spawnPositions[randomIndex3];

        // Generate the pair of colors to be used
        int colorIndex = Random.Range(0, normalColors.Length);

        //Generate the targets

        //------------ Target 1
        GameObject target = Instantiate(targetPrefab, spawnPosition1, rotation);

        // Set Normal Color
        var targetRenderer = target.GetComponent<Renderer>();
        targetRenderer.material.SetColor("_Color", normalColors[colorIndex]);

        target.tag = "Target";
        
        TargetCollisionDetector collisionDetector = target.AddComponent<TargetCollisionDetector>();
        collisionDetector.Initialize(this);
        spawnedTargets.Add(target);  
        
        collisionDetector.SetNormalTarget();



        //------------ Target 2
        target = Instantiate(targetPrefab, spawnPosition2, rotation);

        // Set Normal Color
        targetRenderer = target.GetComponent<Renderer>();
        targetRenderer.material.SetColor("_Color", normalColors[colorIndex]);

        // Set Special Tag
        target.tag = "Target";
        TargetCollisionDetector collisionDetector2 = target.AddComponent<TargetCollisionDetector>();
        collisionDetector2.Initialize(this);
        spawnedTargets.Add(target);         
        collisionDetector2.SetNormalTarget();

        //------------ Target 3
        target = Instantiate(targetPrefab, spawnPosition3, rotation);

        // Set Special Color
        targetRenderer = target.GetComponent<Renderer>();
        targetRenderer.material.SetColor("_Color", specialColors[colorIndex]);

        // Set Special Tag
        target.tag = "Target_Special";
        TargetCollisionDetector collisionDetector3 = target.AddComponent<TargetCollisionDetector>();
        collisionDetector3.Initialize(this);
        spawnedTargets.Add(target);             
        collisionDetector3.SetSpecialTarget();

    }



    public void decreaseCollisionCount()//To increase the number of collisions
    {
        collisionCount--;
        Debug.Log("Points: " + collisionCount); // Output at the console.
    }
    public void increaseCollisionCount()//To increase the number of collisions
    {
        collisionCount++;
        Debug.Log("Points: " + collisionCount); // Output at the console.
    }



}
