using UnityEngine;
using UnityEngine.UI;

public class RepairManager : MonoBehaviour
{
    public Image repairBar;
    public Image backGroundImage;

    public Vector2 padding = new Vector2(10, 10); 

    void Start()
    {
        RectTransform rectTransform1 = backGroundImage.GetComponent<RectTransform>();
        rectTransform1.anchorMin = new Vector2(0, 1);
        rectTransform1.anchorMax = new Vector2(0, 1);
        rectTransform1.pivot = new Vector2(0, 1);
        rectTransform1.anchoredPosition = new Vector2(padding.x, -padding.y);
    }

    void Update()
    {
        repairBar.fillAmount = GameManager.getRepair();
    }
}
