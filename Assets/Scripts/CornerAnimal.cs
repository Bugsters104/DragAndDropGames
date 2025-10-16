using UnityEngine;

public class CornerAnimal : MonoBehaviour
{
    public float jumpHeight = 30f;
    public float jumpDuration = 0.3f;
    public float waitTime = 2f;

    private RectTransform rect;
    private Vector2 startPos;
    private float timer;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        startPos = rect.anchoredPosition;
        timer = waitTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            StartCoroutine(Jump());
            timer = waitTime;
        }
    }

    System.Collections.IEnumerator Jump()
    {
        float elapsed = 0;
        while (elapsed < jumpDuration)
        {
            float yOffset = Mathf.Sin((elapsed / jumpDuration) * Mathf.PI) * jumpHeight;
            rect.anchoredPosition = startPos + new Vector2(0, yOffset);
            elapsed += Time.deltaTime;
            yield return null;
        }
        rect.anchoredPosition = startPos;
    }
}
