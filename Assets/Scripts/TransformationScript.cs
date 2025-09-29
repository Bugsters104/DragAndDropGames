using UnityEngine;

public class TransformationScript : MonoBehaviour
{
    public ObjectScript objScript;

    void Update()
    {
        if (ObjectScript.lastDragged != null)
        {
            RectTransform rect = ObjectScript.lastDragged.GetComponent<RectTransform>();

            if (Input.GetKey(KeyCode.Z))
            {
                rect.transform.Rotate(0, 0, Time.deltaTime * 20f);
            }

            if (Input.GetKey(KeyCode.X))
            {
                rect.transform.Rotate(0, 0, -Time.deltaTime * 20f);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (rect.transform.localScale.y < 1.2f)  // Расширили max
                    rect.transform.localScale = new Vector3(rect.transform.localScale.x, rect.transform.localScale.y + 0.01f, 1f);  // Ускорили шаг
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (rect.transform.localScale.y > 0.2f)  // Расширили min
                    rect.transform.localScale = new Vector3(rect.transform.localScale.x, rect.transform.localScale.y - 0.01f, 1f);  // Ускорили шаг
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (rect.transform.localScale.x > 0.2f)  // Расширили min
                    rect.transform.localScale = new Vector3(rect.transform.localScale.x - 0.01f, rect.transform.localScale.y, 1f);  // Ускорили шаг
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (rect.transform.localScale.x < 1.2f)  // Расширили max
                    rect.transform.localScale = new Vector3(rect.transform.localScale.x + 0.01f, rect.transform.localScale.y, 1f);  // Ускорили шаг
            }
        }
    }
}