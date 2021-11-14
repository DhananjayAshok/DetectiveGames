using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourtStatementToggleScript : MonoBehaviour
{

    public Text statementNameText;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void UpdateToggleInfo(string suspectName)
    {
        statementNameText.text = suspectName;
    }

    public void ButtonClicked()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
