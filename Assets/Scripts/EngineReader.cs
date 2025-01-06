using UnityEngine;
using UnityEngine.InputSystem;

public class EngineReader : MonoBehaviour
{
    private bool Engine; // Number of fuel objects detected
    private InputAction InteractEngineContainer;
    [SerializeField] private GameObject mainShip;
    private Transform EnginePosition;
    void Start(){
        EnginePosition = mainShip.transform.Find("enginePos").GetComponent<Transform>();
    }

    public void InsertEngine(){
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 6f)){
            if (hit.collider.gameObject == gameObject){
                Debug.Log("Engine container detected");
                if (Engine){
                    Dialogue.instance.InitDialogue(new string[]{"I have already inserted the engine!"});
                } else {
                    GameObject obj = Camera.main.gameObject.GetComponent<PickUpScript>().getHeldObj;
                    if(obj.GetComponent<Item>().ItemName == "Engine"){
                        Camera.main.gameObject.GetComponent<PickUpScript>().dropHeldObj();
                        obj.transform.position = EnginePosition.position;
                        obj.transform.rotation = EnginePosition.rotation;
                        obj.tag = "Untagged";
                        Engine = true;
                        GameManager.submitEngine();
                    } else {
                        Dialogue.instance.InitDialogue(new string[]{"I need to put in an Engine!"});
                    }
                }
            }
        }
    }
}
