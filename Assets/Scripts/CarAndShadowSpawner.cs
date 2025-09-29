using System.Collections.Generic;
using UnityEngine;

public class CarAndShadowSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public List<GameObject> carPrefabs;      
    public List<GameObject> shadowPrefabs;  

    [Header("Spawn Points")]
    public Transform spawnPointsParent;
    private Transform[] spawnPoints;

    [Header("Randomization")]
    public bool randomRotation = true;
    public float minScale = 0.2f;  
    public float maxScale = 1.2f;  

    [Header("Scene scripts")]
    private ScreenBoundriesScript screenBou;
    private ObjectScript objManager;  

    void Awake()
    {
        screenBou = FindObjectOfType<ScreenBoundriesScript>();
        objManager = FindObjectOfType<ObjectScript>(); 
        if (objManager == null)
        {
            Debug.LogError("ObjectScript не найден! Проверьте Script Holder.");
            return;
        }

        List<Transform> list = new List<Transform>();
        foreach (Transform t in spawnPointsParent) list.Add(t);
        spawnPoints = list.ToArray();

        if (carPrefabs.Count != shadowPrefabs.Count)
        {
            Debug.LogError("Количество машин и теней должно совпадать!");
            return;
        }

        SpawnAll();
    }

    void SpawnAll()
    {
        if (spawnPoints.Length < carPrefabs.Count + shadowPrefabs.Count)
        {
            Debug.LogWarning("Мало spawn points!");
        }

        List<int> indices = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++) indices.Add(i);
        Shuffle(indices);

        List<GameObject> spawnedCars = new List<GameObject>(); 

        for (int i = 0; i < shadowPrefabs.Count; i++)
        {
            SpawnAtIndex(indices[i + carPrefabs.Count], shadowPrefabs[i], false);
        }

        for (int i = 0; i < carPrefabs.Count; i++)
        {
            GameObject car = SpawnAtIndex(indices[i], carPrefabs[i], true);
            if (car != null) spawnedCars.Add(car);
        }

        objManager.vehicles = spawnedCars.ToArray();
        objManager.InitializePositions();
    }

    GameObject SpawnAtIndex(int index, GameObject prefab, bool isCar, GameObject relatedCar = null)
    {
        if (index >= spawnPoints.Length) return null;

        Transform spawnT = spawnPoints[index];
        GameObject inst = Instantiate(prefab, spawnT.position, Quaternion.identity, spawnPointsParent);

        if (isCar)
        {
            var drag = inst.GetComponent<DragAndDropScript>();
            if (drag != null)
            {
                drag.screenBou = screenBou;
                drag.objectScr = objManager;  
            }
        }
        else
        {
            var drop = inst.GetComponent<DropPlaceScript>();
            if (drop != null)
            {
                drop.objScript = objManager;  
            }
        }

        if (randomRotation) inst.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

        float scaleX = Random.Range(minScale, maxScale);
        float scaleY = Random.Range(minScale, maxScale);

        inst.transform.localScale = new Vector3(scaleX, scaleY, 1f);

        return inst;
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            T tmp = list[r];
            list[r] = list[i];
            list[i] = tmp;
        }
    }
}