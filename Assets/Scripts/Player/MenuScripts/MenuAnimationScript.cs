using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class MenuAnimationScript : MonoBehaviour
{
    [Header("Internal Variables (Can ignore)")]
    public Animator panelAnimator;
    public Animator scrollviewAnimator;
    [HideInInspector]
    public bool menuOpen = false;

    GameObject panel;
    GameObject scrollview;
    Vector3 zeroScale = new Vector3(0, 0, 0);
    Vector3 oneScale = new Vector3(1, 1, 1);
    // Start is called before the first frame update
    void Start()
    {
        panel = panelAnimator.gameObject;
        scrollview = scrollviewAnimator.gameObject;
    }

    void Disappear(Animator animator) {
        animator.SetBool("isExpanded", false);
    }

    void Emerge(Animator animator) {
        animator.SetBool("isExpanded", true);
    }

    public void SelectClueAnimation() {
        Disappear(scrollviewAnimator);
        Emerge(panelAnimator);
    }

    public void DeselectClueAnimation() {
        Disappear(panelAnimator);
        Emerge(scrollviewAnimator);
    }

    public void OpenMenu() {
        menuOpen = true;
        SetMenuOpen();

    }

    public void CloseMenu() {
        menuOpen = false;
        SetMenuOpen();
        ForceScale();
    }

    public void SetMenuOpen() {
        scrollviewAnimator.SetBool("menuOpen", menuOpen);
        panelAnimator.SetBool("menuOpen", menuOpen);
    }

    public void ForceScale() {
        panel.transform.localScale = zeroScale;
        scrollview.transform.localScale = oneScale;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
