using UnityEngine;
using UnityEngine.UI;

public class FuelBarManager : MonoBehaviour
{
    public Image fuelBar;
    public Image backGroundImage;

    public Vector2 padding = new Vector2(10, 10);

    void Start()
    {
        RectTransform rectTransform1 = backGroundImage.GetComponent<RectTransform>();
        rectTransform1.anchorMin = new Vector2(1, 1);
        rectTransform1.anchorMax = new Vector2(1, 1);
        rectTransform1.pivot = new Vector2(1, 1);
        rectTransform1.anchoredPosition = new Vector2(-padding.x, -padding.y);
    }

    void Update()
    {
        fuelBar.fillAmount = (GameManager.getFuel() * (float) 0.33);
        if (GameManager.getFuel() == 3) { fuelBar.fillAmount = 1; }
        Debug.Log(GameManager.getFuel());
    }
}
