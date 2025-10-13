using UnityEngine;

public class PlaceholderSpawnerScript : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject[] placeholders;
    public GameObject placeholderFolder;
    public ObjectScript objectScript;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        GameObject[] selectedSpots = new GameObject[spawnPoints.Length];
        spawnPoints.CopyTo(selectedSpots, 0);

        System.Random rnd = new System.Random();

        int n = selectedSpots.Length;
        while (n > 1)
        {//  oldArray.Skip(1).Take(oldArray.Length - 2).ToArray();
            int k = rnd.Next(n--);
            GameObject temp = selectedSpots[n];
            selectedSpots[n] = selectedSpots[k];
            selectedSpots[k] = temp;
        }

        for (int i = 0; i < objectScript.vehicles.Length; i++)
        {
            if (i > placeholders.Length - 1)
            {
                break;
            }

            GameObject placeholderPrefab = placeholders[i];

            Vector3 spawnPosition = new Vector3(selectedSpots[i].transform.position.x, selectedSpots[i].transform.position.y, selectedSpots[i].transform.position.z);

            placeholderPrefab.GetComponent<DropPlaceScript>().objScript = objectScript;

            //GameObject placedPrefab = Instantiate(placeholderPrefab, spawnPosition, Quaternion.identity, placeholderFolder.transform);
            Instantiate(placeholderPrefab, spawnPosition, Quaternion.identity, placeholderFolder.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
