using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int StoryEvent;
    private StoryManager storyManager;
    void Start()
    {
        storyManager = GameObject.Find("StoryManager").GetComponent<StoryManager>();
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            storyManager.AdvanceStoryEvent(StoryEvent);
            // if (storyManager.storyEvent > StoryEvent){
            //     gameObject.SetActive(false);
            // }
        }
    }
}
