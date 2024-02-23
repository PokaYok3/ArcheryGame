using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text collisionCountText;
    public Text roundsCountText;
    public Text totalRoundsText;
    public TargetSpawner targetSpawner;
    void Start()
    {
        // Obtener la referencia al script TargetSpawner
        targetSpawner = GameObject.FindObjectOfType<TargetSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        collisionCountText.text = "Collision Count: " + targetSpawner.collisionCount.ToString();
        roundsCountText.text = "Rounds: " + targetSpawner.roundsCount.ToString() + " / " + targetSpawner.TotalRounds.ToString();
        totalRoundsText.text = "Total Rounds: " + targetSpawner.TotalRounds.ToString();
        
    }
    
}
