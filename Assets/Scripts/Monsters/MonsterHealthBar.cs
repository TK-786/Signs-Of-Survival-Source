using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{
    [Header("References")]
    public GameObject monster;
    public Slider healthBarSlider;
    public Vector3 offset = new Vector3(0, 1, 0); // Position offset to raise the health bar above the monster

    private void Update()
    {
        // Position the health bar above the monster, applying the offset
        transform.position = monster.transform.position + offset;

        // Make the health bar face the camera
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);

        // Update the health bar slider value
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        // Get the monster's health ratio from its script
        var monsterScript = monster.GetComponent<BlindMonsterAI>();

        if (monsterScript != null)
        {
            healthBarSlider.value = monsterScript.GetHealthRatio();
        }
    }
}
