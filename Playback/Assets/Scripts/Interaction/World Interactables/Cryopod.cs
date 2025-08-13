using UnityEngine;
using UnityEngine.SceneManagement;

public class Cryopod : MonoBehaviour
{
    public LayerMask playerLayer;

    private float cryoTimer = 3f;
    private float timer = 0;

    private bool playerIn = false;
    private bool frozen = false;

    [SerializeField] private Material frostMat;

    public int nextLevelIndex;

    //door
    [SerializeField] private GameObject door;
    private Vector3 openPos;
    [SerializeField] private Vector3 closedPos;    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.instance.inputEnabled = true;
        frostMat.SetFloat("_GeneralAlpha", 0); //make sure the material is properly frozen
        openPos = door.transform.localPosition;
        timer = cryoTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIn && !frozen)
        {
            timer += Time.deltaTime;
            frostMat.SetFloat("_GeneralAlpha", (timer / cryoTimer));
            door.transform.localPosition = Vector3.Lerp(openPos, closedPos, (timer / 0.5f));
            if(timer >= cryoTimer)
            {
                InputManager.instance.inputEnabled = false;
                frozen = true;
                frostMat.SetFloat("_GeneralAlpha", 1); //make sure the material is properly frozen
                Invoke("LoadNextLevel", 1.5f);
            }
        }
        else if(!playerIn && timer > 0)
        {
            timer -= Time.deltaTime;
            frostMat.SetFloat("_GeneralAlpha", (timer / cryoTimer));
            door.transform.localPosition = Vector3.Lerp(openPos, closedPos, (timer / 0.5f));
        }
    }

    private void LoadNextLevel()
    {
        //make sure old ghost UI is destroyed
        GameUI.instance.DestroyGhostUI();
        SceneManager.LoadScene(nextLevelIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        if((playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            playerIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            playerIn = false;
        }
    }
}
