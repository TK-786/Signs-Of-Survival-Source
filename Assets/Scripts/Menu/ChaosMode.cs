//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ChaosMode : MonoBehaviour
//{
//    public float PlayerHealth { get; private set; }
//    public float JumpForce { get; private set; }
//    public float RunningSpeed { get; private set; }
//    public float Damage { get; private set; }
//    public float MonsterSpeed { get; private set; }
//    //public float Gravity { get; private set; }



//    public Slider playerHealthSlider;
//    public Slider jumpForceSlider;
//    public Slider runningSpeedSlider;
//    public Slider damageSlider;
//    public Slider monsterSpeedSlider;
//    //public Toggle gravityToggle;
//    // Start is called before the first frame update
//    void Start()
//    {
//        PlayerHealth = PlayerHealthSlider.value;
//        JumpForce = JumpForceSlider.value;
//        RunningSpeed = RunningSpeedSlider.value;
//        Damage = DamageSlider.value;
//        MonsterSpeed = MonsterSpeedSlider.value;
//        //Gravity = GravityToggle.value;


//        playerHealthSlider.onValueChanged.AddListener(value => PlayerHealth = value);
//        jumpForceSlider.onValueChanged.AddListener(value => JumpForce = value);
//        runningSpeedSlider.onValueChanged.AddListener(value => RunningSpeed = value);
//        damageSlider.onValueChanged.AddListener(value => Damage = value);
//        monsterSpeedSlider.onValueChanged.AddListener(value => MonsterSpeed = value);
//        //gravityToggle.onValueChanged.AddListener(value => Gravity = value);

//    }

//    //// Update is called once per frame
//    //void Update()
//    //{

//    //}
//}
