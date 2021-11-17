using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuProgress : MonoBehaviour
{
    // Start is called before the first frame update
    public void MenuAdvanceScene()
    {
        SceneManager.LoadScene(1);
    }
}
