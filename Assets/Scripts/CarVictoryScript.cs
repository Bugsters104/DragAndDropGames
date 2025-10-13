using UnityEngine;
using UnityEngine.UI;

public class CarVictoryScript : MonoBehaviour
{
    public GameObject counterTextObject;

    private static Text counterText;

    [HideInInspector]
    public static bool lostCar;

    [HideInInspector]
    public static int counter;

    [HideInInspector]
    public static int max;

    private static int realMax;

    public GameObject carContainer;

    private static void checkVictory() {

       // counterText.GetComponent<Text>().text = counter +" / " + realMax;

        //counterText.GetComponent<Text>().text = "0 / " + realMax;
        counterText.text = counter + " / " + realMax;

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

        counterText = counterTextObject.GetComponent<Text>();
        counterText.text = "0 / " + realMax;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
