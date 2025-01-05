using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCueManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static AudioCueManager audioCueManager;
    private GameObject player;
    private RectTransform cueUI;
    private float radius = 350f;
    [SerializeField] List<AudioClip> audioList = new List<AudioClip>();
    [SerializeField] List<GameObject> cueIcon = new List<GameObject>();
    private Dictionary<AudioClip, GameObject> clipToCue = new Dictionary<AudioClip, GameObject>();
    private Dictionary<AudioSource, GameObject> activeCues = new Dictionary<AudioSource, GameObject>();
    public Dictionary<AudioSource, GameObject> audibleSources = new Dictionary<AudioSource, GameObject>();
    void Awake()
    {
        if (audioCueManager==null){
            audioCueManager = this;
        }else {
            Destroy(gameObject);
        }
        player = GameObject.Find("Player");
        cueUI = gameObject.GetComponent<RectTransform>();
        for(int i = 0; i < audioList.Count; i ++){
            clipToCue[audioList[i]] = cueIcon[i];
        }
    }

    // Update is called once per frame

    public void displayVisualCue(AudioSource source, GameObject sourceObject){
        float distance = Vector3.Distance(sourceObject.transform.position, player.transform.position); 
        Vector3 direction = (sourceObject.transform.position - player.transform.position).normalized;
        float angle = Vector3.SignedAngle(player.transform.forward, direction, Vector3.up);
        float volume = source.volume * source.GetCustomCurve(AudioSourceCurveType.CustomRolloff).Evaluate(distance / source.maxDistance);
        angle = -angle + 90;
        //Unity's UI coordinate system uses a clockwise rotation convention, while Vector3.SignedAngle works in a counterclockwise system. You need to invert the angle to account for this.
        Vector2 cuePosition = new Vector2(
        Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
        Mathf.Sin(angle * Mathf.Deg2Rad) * radius);

        if (source.GetComponent<audioSource>().hasAudioChanged && activeCues.ContainsKey(source)){
            Destroy(activeCues[source]);
            activeCues.Remove(source);
            source.GetComponent<audioSource>().acknowledgeAudioChange();
        }

        if (!activeCues.ContainsKey(source)){
            GameObject cue = Instantiate(clipToCue[source.GetComponent<audioSource>().getCueType], cueUI); // Instantiate the cue prefab
            activeCues[source] = cue;
            source.GetComponent<audioSource>().acknowledgeAudioChange();
        }

        activeCues[source].GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, Mathf.Clamp01(volume)); // Fade by volume
        RectTransform cueTransform = activeCues[source].GetComponent<RectTransform>();
        cueTransform.anchoredPosition = cuePosition;

    }
    void Update()
    {
        foreach (var entry in audibleSources){
            displayVisualCue(entry.Key, entry.Value);
        }

        List<AudioSource> toRemove = new List<AudioSource>();
        foreach (var cue in activeCues){
            if (!audibleSources.ContainsKey(cue.Key)){
                Destroy(cue.Value);
                toRemove.Add(cue.Key);
            }
        }
        foreach (var source in toRemove){
            activeCues.Remove(source);
        }
    }
}

