using UnityEngine;
using TMPro;

public class RainbowText : MonoBehaviour
{
    public float colorChangeSpeed = 1.0f; // Speed of color change

    private TextMeshProUGUI textMeshPro;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // use raeltime to solve the text not changing color
        float hue = Mathf.Sin(Time.realtimeSinceStartup * colorChangeSpeed) * 0.5f + 0.5f;

        textMeshPro.color = Color.HSVToRGB(hue, 1.0f, 1.0f);
    }
}
