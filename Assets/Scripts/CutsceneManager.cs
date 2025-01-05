using UnityEngine;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.InputSystem;


public class CutsceneManager : MonoBehaviour
{
    public GameObject crashCutsceneCamera; // Camera for the crash cutscene
    public GameObject finalCutsceneCamera; // Camera for the final cutscene
    public GameObject mainCamera;          // Player's Main Camera
    public PlayableDirector crashDirector; // Timeline for the crash cutscene
    public PlayableDirector finalDirector; // Timeline for the final cutscene
    public Canvas crashCutsceneCanvas;     // Canvas for the crash cutscene subtitles
    public Canvas finalCutsceneCanvas;     // Canvas for the final cutscene subtitles

    void Start()
    {
        crashCutsceneCanvas.gameObject.SetActive(false);
        finalCutsceneCanvas.gameObject.SetActive(false);


        crashCutsceneCamera.SetActive(false);
        finalCutsceneCamera.SetActive(false);
        mainCamera.SetActive(true);



        crashDirector.stopped += OnCrashCutsceneEnd;
        PlayCrashCutscene();



        finalDirector.stopped += OnFinalCutsceneEnd;

    }

    void Update()
    {
        // Temporary input for testing the final cutscene
        if (Keyboard.current.bKey.wasPressedThisFrame) // Press 'B' to play the final cutscene
        {
            Debug.Log("Playing final cutscene...");
            PlayFinalCutscene();
        }
    }

    public void PlayCrashCutscene()
    {
        // Play the crash cutscene
        crashCutsceneCamera.SetActive(true);
        mainCamera.SetActive(false);
        crashCutsceneCanvas.gameObject.SetActive(true);
        crashDirector.Play();
    }

    public void PlayFinalCutscene()
    {
        // Play the final cutscene
        finalCutsceneCamera.SetActive(true);
        mainCamera.SetActive(false);
        finalCutsceneCanvas.gameObject.SetActive(true);
        finalDirector.Play();
    }

    private void OnCrashCutsceneEnd(PlayableDirector dir)
    {
        // Reset after crash cutscene ends
        crashCutsceneCamera.SetActive(false);
        mainCamera.SetActive(true);
        crashCutsceneCanvas.gameObject.SetActive(false);
        Debug.Log("Crash cutscene finished. Gameplay begins!");
    }

    private void OnFinalCutsceneEnd(PlayableDirector dir)
    {
        // Reset after final cutscene ends
        finalCutsceneCamera.SetActive(false);
        finalCutsceneCanvas.gameObject.SetActive(false);
        Debug.Log("Final cutscene finished. Game ends!");
        // Trigger end-game sequence, credits, etc.
    }


}