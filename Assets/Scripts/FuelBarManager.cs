using UnityEngine;
using UnityEngine.UI;

public class FuelBarManager : MonoBehaviour
{
    public Image fuelBar;
    public Image backGroundImage;
    public Vector2 padding = new Vector2(10, 10);

    // Sets up the background image's position and anchors on start
    void Start()
    {
        RectTransform rectTransform1 = backGroundImage.GetComponent<RectTransform>();
        rectTransform1.anchorMin = new Vector2(1, 1);
        rectTransform1.anchorMax = new Vector2(1, 1);
        rectTransform1.pivot = new Vector2(1, 1);
        rectTransform1.anchoredPosition = new Vector2(-padding.x, -padding.y);
    }

    // Updates the fuel bar's fill amount based on the current fuel level
    void Update()
    {
        fuelBar.fillAmount = (GameManager.getFuel() * 0.33f);
        if (GameManager.getFuel() == 3) { fuelBar.fillAmount = 1; }
    }
}
