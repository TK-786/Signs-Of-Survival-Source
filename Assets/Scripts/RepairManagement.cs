using UnityEngine;
using UnityEngine.UI;

public class RepairManager : MonoBehaviour
{
    public Image repairBar; // UI element representing the repair bar
    public Image backGroundImage; // Background image for the repair bar

    public Vector2 padding = new Vector2(10, 10); // Padding for the background image position

    void Start()
    {
        // Configures the background image's position and anchors
        RectTransform rectTransform1 = backGroundImage.GetComponent<RectTransform>();
        rectTransform1.anchorMin = new Vector2(0, 1);
        rectTransform1.anchorMax = new Vector2(0, 1);
        rectTransform1.pivot = new Vector2(0, 1);
        rectTransform1.anchoredPosition = new Vector2(padding.x, -padding.y);
    }

    void Update()
    {
        // Updates the fill amount of the repair bar based on the current repair status
        repairBar.fillAmount = GameManager.getRepair();
    }
}
