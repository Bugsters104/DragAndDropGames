using System;
using UnityEngine;
using UnityEngine.UI;

public class TickerScript : MonoBehaviour
{
    public GameObject TickerText;

    private Text tickerTextString;
    private float timeTicked = 0;

    public static bool mustTick = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tickerTextString = TickerText.GetComponent<Text>();
        timeTicked = 0;
        mustTick = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!mustTick) return;

        timeTicked += Time.deltaTime;

        TimeSpan time = TimeSpan.FromSeconds((int)timeTicked);

        tickerTextString.text = time.ToString(@"hh\:mm\:ss");
        
    }
}
