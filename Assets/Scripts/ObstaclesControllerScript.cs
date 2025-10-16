using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ObstaclesControllerScript : MonoBehaviour
{
    [HideInInspector]
    public float speed = 1f;
    public float waveAmplitude = 25f;
    public float waveFrequency = 1f;
    public float fadeDuration = 1.5f;

    private ObjectScript objectScript;
    private ScreenBoundriesScript screenBoundriesScript;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private bool isFadingOut = false;
    private Image image;
    private Color orginalColor;
    private bool isExploding = false;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        orginalColor = image.color;

        objectScript = Object.FindFirstObjectByType<ObjectScript>();
        screenBoundriesScript = Object.FindFirstObjectByType<ScreenBoundriesScript>();

        StartCoroutine(FadeIn());

        if (TryGetComponent<CircleCollider2D>(out var col))
            col.isTrigger = true;

        if (TryGetComponent<Animator>(out Animator anim))
            anim.SetBool("explode", false);
    }

    void Update()
    {
        float waveOffset = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
        rectTransform.anchoredPosition += new Vector2(-speed * Time.deltaTime, waveOffset * Time.deltaTime);

        if (!isFadingOut)
        {
            if ((speed > 0 && transform.position.x < screenBoundriesScript.minX + 80) ||
                (speed < 0 && transform.position.x > screenBoundriesScript.maxX - 80))
            {
                isFadingOut = true;
                StartCoroutine(FadeOutAndDestroy());
            }
        }

        if (CompareTag("Bomb") && !isExploding &&
            RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, Camera.main))
        {
            TriggerExplosion();
        }

        if (ObjectScript.drag && !isFadingOut &&
            RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, Camera.main))
        {
            if (ObjectScript.lastDragged != null)
            {
                StartCoroutine(ShrinkAndDestroy(ObjectScript.lastDragged, 0.5f));
                ObjectScript.lastDragged = null;
                ObjectScript.drag = false;
                objectScript.ShowDefeat();
            }

            if (CompareTag("Bomb"))
                StartToDestroy(Color.red);
            else
                StartToDestroy(Color.cyan);
        }
    }

    public void TriggerExplosion()
    {
        if (isExploding) return;
        isExploding = true;

        image.color = Color.red; 

        if (TryGetComponent<Animator>(out Animator animator))
        {
            animator.SetBool("explode", true);
        }

        objectScript.effects.PlayOneShot(objectScript.audioCli[15], 5f);
        StartCoroutine(Vibrate());

        float radius = 0f;
        if (TryGetComponent<CircleCollider2D>(out CircleCollider2D circleCollider))
            radius = circleCollider.radius * transform.lossyScale.x;

        ExplodeAndDestroyNearbyObjects(radius);

        StartCoroutine(ResetBool());

        StartCoroutine(FadeOutAndDestroyDelayed(1.2f));
    }

    IEnumerator ResetBool()
    {
        yield return new WaitForSeconds(0.3f);
        if (TryGetComponent<Animator>(out Animator animator))
            animator.SetBool("explode", false);
    }

    IEnumerator FadeOutAndDestroyDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(FadeOutAndDestroy());
    }

    void ExplodeAndDestroyNearbyObjects(float radius)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D hit in hits)
        {
            if (hit == null || hit.gameObject == gameObject) continue;

            if (!hit.CompareTag("Bomb"))
            {
                if (hit.TryGetComponent<ObstaclesControllerScript>(out var obj))
                {
                    obj.StartToDestroy(Color.cyan);
                }
                else
                {
                    Destroy(hit.gameObject);
                }
            }
        }
    }

    public void StartToDestroy(Color c)
    {
        if (isFadingOut || isExploding) return;

        isFadingOut = true;
        image.color = c;
        StartCoroutine(RecoverColor(0.5f));
        StartCoroutine(Vibrate());
        objectScript.effects.PlayOneShot(objectScript.audioCli[14]);
        StartCoroutine(FadeOutAndDestroy());
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    IEnumerator FadeOutAndDestroy()
    {
        float t = 0f;
        float startAlpha = canvasGroup.alpha;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        Destroy(gameObject);
    }

    IEnumerator ShrinkAndDestroy(GameObject target, float duration)
    {
        Vector3 originalScale = target.transform.localScale;
        Quaternion originalRotation = target.transform.rotation;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            target.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t / duration);
            float angle = Mathf.Lerp(0f, 360f, t / duration);
            target.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            yield return null;
        }
        Destroy(target);
    }

    IEnumerator RecoverColor(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        image.color = orginalColor;
    }

    IEnumerator Vibrate()
    {
        Vector2 originalPosition = rectTransform.anchoredPosition;
        float duration = 0.3f;
        float elapsed = 0f;
        float intensity = 5f;

        while (elapsed < duration)
        {
            rectTransform.anchoredPosition = originalPosition + Random.insideUnitCircle * intensity;
            elapsed += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = originalPosition;
    }

    void OnDrawGizmosSelected()
    {
        if (TryGetComponent<CircleCollider2D>(out CircleCollider2D col))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, col.radius * transform.lossyScale.x);
        }
    }
}
