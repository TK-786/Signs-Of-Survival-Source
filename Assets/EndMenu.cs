using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public Animator creditsAnimation;
    private bool animationPlayed = false;
    public AudioSource endMenuMusic;
    // Start is called before the first frame update
    void Start()
    {
        if (creditsAnimation != null)
        {
            creditsAnimation.Play("Credits Animation");
        }
        if (endMenuMusic != null && !endMenuMusic.isPlaying)
        {
            endMenuMusic.Play();
        }

    }
    void Update()
    {
        if (!animationPlayed && creditsAnimation != null)
        {
            AnimatorStateInfo stateInfo = creditsAnimation.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 1 && stateInfo.IsName("Credits Animation")) 
            {
                animationPlayed = true;
                ShowMainMenu();
            }
        }
    }

    private void ShowMainMenu()
    {
        if (endMenuMusic != null && endMenuMusic.isPlaying)
        {
            endMenuMusic.Stop();
        }
        if (MainMenu != null)
        {
            MainMenu.SetActive(true); 
        }

        gameObject.SetActive(false); 
    }
}
