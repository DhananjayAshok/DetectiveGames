using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueScript : MonoBehaviour
{
    [TextArea]
    public string clueString;
    public string clueName;
    public string clueLocation;
    [TextArea]
    public string clueAutopsy;
    public Sprite clueSprite;
    public bool discovered = false;
    public bool isTriviallyUseless = false;
    public bool isAutopsySuccess = false;

    // Start is called before the first frame update
    void Start()
    {
        if(clueAutopsy.Equals("") || clueAutopsy.Equals(null)){
            isAutopsySuccess = false;
            clueAutopsy = "There's nothing for me to see here. Quit wasting my time";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
