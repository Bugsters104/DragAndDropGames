using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlaceholderSpawnScript : MonoBehaviour
{

    public Transform[] spawnPoints;
    public GameObject[] placeholders;
    void Start()
    {
        Transform[] selectedSpots = new Transform[spawnPoints.Length];
        spawnPoints.CopyTo(selectedSpots, 0);

        System.Random rnd = new System.Random();

        int n = selectedSpots.Length;
        while (n > 1)
        {//  oldArray.Skip(1).Take(oldArray.Length - 2).ToArray();
            int k = rnd.Next(n--);
            Transform temp = selectedSpots[n];
            selectedSpots[n] = selectedSpots[k];
            selectedSpots[k] = temp;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
