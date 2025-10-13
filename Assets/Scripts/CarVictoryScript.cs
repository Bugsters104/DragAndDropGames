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

        counterText.color = new Color(.8f, .3f, .3f, 1f);

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
