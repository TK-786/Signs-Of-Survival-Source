using UnityEngine;
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
    public GameObject crashCutsceneShip;   // Ship object for the crash cutscene
    public GameObject finalCutsceneShip;   // Ship object for the final cutscene (can be the same as crashCutsceneShip)

    void Start()
    {
        // Ensure only the necessary canvas is active at the start
        crashCutsceneCanvas.gameObject.SetActive(false);
        finalCutsceneCanvas.gameObject.SetActive(false);

        // Set the initial states of the cameras and ships
        crashCutsceneCamera.SetActive(false);
        finalCutsceneCamera.SetActive(false);
        crashCutsceneShip.SetActive(false); // Ship is initially hidden
        finalCutsceneShip.SetActive(false); // Final ship is initially hidden
        mainCamera.SetActive(true);

        // Subscribe to cutscene end events
        crashDirector.stopped += OnCrashCutsceneEnd;
        finalDirector.stopped += OnFinalCutsceneEnd;

        // Start the first cutscene
        PlayCrashCutscene();
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
        // Activate crash cutscene components
        crashCutsceneCamera.SetActive(true);
        crashCutsceneShip.SetActive(true); // Show the crash cutscene ship
        mainCamera.SetActive(false);
        crashCutsceneCanvas.gameObject.SetActive(true);

        crashDirector.Play();
    }

    public void PlayFinalCutscene()
    {
        // Activate final cutscene components
        finalCutsceneCamera.SetActive(true);
        finalCutsceneShip.SetActive(true); // Show the final cutscene ship
        mainCamera.SetActive(false);
        finalCutsceneCanvas.gameObject.SetActive(true);

        finalDirector.Play();
    }

    private void OnCrashCutsceneEnd(PlayableDirector dir)
    {
        // Deactivate crash cutscene components
        crashCutsceneCamera.SetActive(false);
        crashCutsceneShip.SetActive(false); // Hide the crash cutscene ship
        mainCamera.SetActive(true);
        crashCutsceneCanvas.gameObject.SetActive(false);

        Debug.Log("Crash cutscene finished. Gameplay begins!");
    }

    private void OnFinalCutsceneEnd(PlayableDirector dir)
    {
        // Deactivate final cutscene components
        finalCutsceneCamera.SetActive(false);
        finalCutsceneShip.SetActive(false); // Hide the final cutscene ship
        finalCutsceneCanvas.gameObject.SetActive(false);

        Debug.Log("Final cutscene finished. Game ends!");
        // Trigger end-game sequence, credits, etc.
    }
}
