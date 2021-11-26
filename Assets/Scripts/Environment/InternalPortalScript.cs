using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternalPortalScript : MonoBehaviour
{
    GodScript godScript;
    public string targetScene;
    // Start is called before the first frame update
    void Start()
    {
        godScript = GameObject.FindGameObjectWithTag("God").GetComponent<GodScript>();
    }

    public void Activate() {
        godScript.SceneChangeInternal(targetScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
