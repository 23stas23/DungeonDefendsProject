using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Transform> SpawnPoints = new List<Transform>();
    [SerializeField] private int Level;
    [SerializeField] private GameObject player;

    void Start()
    {
        player.transform.position = SpawnPoints[0].position; // Spawn player at first level
    }

    public void Nextlevel()
    {
        Level++;
        //Restar if no more levels
        if (Level >= SpawnPoints.Count) Restart();
        else player.transform.position = SpawnPoints[Level].position;
    }
    

    public void Restart()
    {
        SceneManager.LoadScene(0); //Restart Scene
    }


    
}
