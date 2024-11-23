using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{
    [Header("References")]
    public GameObject monster; 
    public Slider healthBarSlider; 
    public Vector3 offset = new Vector3(0, 2, 0);

    private void Update()
    {
        transform.position = monster.transform.position;


        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        var monsterScript = monster.GetComponent<BlindMonsterAI>();

        healthBarSlider.value = monsterScript.GetHealthRatio();
        
    }
}
