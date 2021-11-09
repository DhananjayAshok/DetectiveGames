using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleClueScript : MonoBehaviour
{
    public GameObject cluePopup;
    public Text clueNameText;
    public Text cluePopupText;
    public Image cluePopupImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool isDisplaying() {
        return cluePopup.activeSelf;
    }

    public void displayClue(GameObject clue) {
        ClueScript cs = clue.GetComponent<ClueScript>();
        cs.discovered = true;
        string clueName = cs.clueName;
        string clueString = cs.clueString;
        Sprite clueSprite = cs.clueSprite;
        cluePopupText.text = clueString;
        clueNameText.text = clueName;
        cluePopupImage.sprite = clueSprite;
        if (isDisplaying())
        {
            closePopup();
        }
        else {
            cluePopup.SetActive(true);
        }
    }

    public void closePopup() {
        cluePopup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
