using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourtClueToggleScript : MonoBehaviour
{

    public Text clueNameText;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void UpdateToggleInfo(string clueName)
    {
        clueNameText.text = clueName;
    }

    public void ButtonClicked()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
