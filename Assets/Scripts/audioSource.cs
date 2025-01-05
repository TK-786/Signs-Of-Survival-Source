using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioSource : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private AudioSource source;
    private float distance;
    private bool isAudible = false;
    AudioCueManager manager;
    private bool audioChanged = false;
    public bool hasAudioChanged => audioChanged;
    [SerializeField] private AudioClip cueType;
    public AudioClip getCueType => cueType;
    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.Find("Player");
        source = GetComponent<AudioSource>();
        manager = AudioCueManager.audioCueManager;
        isAudible = false;
        cueType = source.clip;
    }

    public void acknowledgeAudioChange(){
        audioChanged = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (cueType != source.clip && !audioChanged){
            cueType = source.clip;
            audioChanged = true;
        }
        
        distance = Vector3.Distance(transform.position, player.transform.position);
        float volume = source.volume * source.GetCustomCurve(AudioSourceCurveType.CustomRolloff).Evaluate(distance / source.maxDistance);
        if (volume > 0){
            if (!isAudible && !manager.audibleSources.ContainsKey(source)){
                isAudible=true;
                manager.audibleSources.Add(source, gameObject);
                Debug.Log("source is audible");
            }
        } else {
            if (isAudible && manager.audibleSources.ContainsKey(source)){
                isAudible = false;
                manager.audibleSources.Remove(source);
                Debug.Log("source is inaudible");
            }
        }
    }
}
