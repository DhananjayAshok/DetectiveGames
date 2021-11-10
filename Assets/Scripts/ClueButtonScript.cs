using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueButtonScript : MonoBehaviour
{

    public Text clueNameText;
    public Text clueLocationText;
    public Image cluePopupImage;
    public ClueObject clueObject;
    public GameObject playerAttachment;
    private MenuButtonScript menuButtonScript;

    // Start is called before the first frame update
    void Start()
    {
        playerAttachment = GameObject.FindGameObjectsWithTag("Player Attachment")[0]; // There should be one and only one player attachment in the scene
        menuButtonScript = playerAttachment.GetComponent<MenuButtonScript>();
    }

    public void UpdateButtonInfo(ClueObject co) {
        clueNameText.text = co.clueName;
        clueLocationText.text = co.clueLocation;
        cluePopupImage.sprite = co.clueSprite;
        clueObject = co;
    }

    public void ButtonClicked() {
        menuButtonScript.Activate(clueObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
