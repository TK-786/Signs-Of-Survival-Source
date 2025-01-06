using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public GameObject crashCutsceneCamera; // Camera for the crash cutscene
    public GameObject finalCutsceneCamera; // Camera for the final cutscene
    public GameObject mainCamera;          // Player's Main Camera
    public PlayableDirector crashDirector; // Timeline for the crash cutscene
    public PlayableDirector finalDirector; // Timeline for the final cutscene
    public Canvas crashCutsceneCanvas;     // Canvas for the crash cutscene subtitles
    public Canvas finalCutsceneCanvas;     // Canvas for the final cutscene subtitles
    public GameObject crashCutsceneShip;   // Ship object for the crash cutscene
    public GameObject mainShip;            // MainShipOpen prefab (used during gameplay)

    private static bool hasCrashCutscenePlayed = false; // Tracks if the crash cutscene has already played

    void Start()
    {
        // Ensure only the necessary canvas is active at the start
        crashCutsceneCanvas.gameObject.SetActive(false);
        finalCutsceneCanvas.gameObject.SetActive(false);

        // Set the initial states of the cameras and ships
        crashCutsceneCamera.SetActive(false);
        finalCutsceneCamera.SetActive(false);
        crashCutsceneShip.SetActive(false);
        mainShip.SetActive(true);
        mainCamera.SetActive(true);

        // Subscribe to cutscene end events
        crashDirector.stopped += OnCrashCutsceneEnd;
        finalDirector.stopped += OnFinalCutsceneEnd;

        // Play the crash cutscene only if it hasn't already been played
        if (!hasCrashCutscenePlayed)
        {
            PlayCrashCutscene();
        }
    }

    public void PlayCrashCutscene()
    {
        hasCrashCutscenePlayed = true; // Mark the crash cutscene as played

        // Activate crash cutscene components
        crashCutsceneCamera.SetActive(true);
        crashCutsceneShip.SetActive(true); // Show the crash cutscene ship
        mainShip.SetActive(false);         // Hide the main ship during the cutscene
        mainCamera.SetActive(false);
        crashCutsceneCanvas.gameObject.SetActive(true);

        AudioSource audioSource = mainShip.GetComponent<AudioSource>();
        audioSource.Pause();

        crashDirector.Play();
    }

    public void PlayFinalCutscene()
    {
        // Activate final cutscene components
        finalCutsceneCamera.SetActive(true);
        mainShip.SetActive(false);         // Hide the main ship during the cutscene
        mainCamera.SetActive(false);
        finalCutsceneCanvas.gameObject.SetActive(true);

        finalDirector.Play();
    }

    private void OnCrashCutsceneEnd(PlayableDirector dir)
    {
        // Deactivate crash cutscene components
        crashCutsceneCamera.SetActive(false);
        crashCutsceneShip.SetActive(false); // Hide the crash cutscene ship
        mainShip.SetActive(true);           // Show the main ship after the cutscene
        mainCamera.SetActive(true);
        crashCutsceneCanvas.gameObject.SetActive(false);

        Debug.Log("Crash cutscene finished. Gameplay begins!");
        AudioSource audioSource = mainShip.GetComponent<AudioSource>();
        audioSource.UnPause();
    }
    

    private void OnFinalCutsceneEnd(PlayableDirector dir)
    {
        // Deactivate final cutscene components
        finalCutsceneCamera.SetActive(false);
        finalCutsceneCanvas.gameObject.SetActive(false);
        mainShip.SetActive(true); // Show the main ship after the final cutscene

        Debug.Log("Final cutscene finished. Game ends!");
        // Trigger end-game sequence, credits, etc.
    }
}
