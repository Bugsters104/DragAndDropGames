using UnityEngine;

public class CarVictoryScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static bool lostCar;

    [HideInInspector]
    public static int counter;

    [HideInInspector]
    public static int max;

    private static int realMax;

    public GameObject carContainer;

    private static void checkVictory() {

        Debug.Log(counter + " / " + realMax);

        if (counter != realMax) return;

        if (lostCar) {
            Debug.Log("Nooooooooooooo!");                        
        } else {
            Debug.Log("Victory!");
        }
    }

    public static  void decreaseMax() {
        lostCar = true;
        realMax--;

        checkVictory();
    }

    public static void increment() {
        counter++;

        checkVictory();
    }

    void Start()
    {
        max = carContainer.GetComponent<Transform>().childCount;
        realMax = max;
        counter = 0;
        lostCar = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
