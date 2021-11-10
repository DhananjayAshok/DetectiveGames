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
    // Start is called before the first frame update
    void Start()
    {
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
    }

    public void SetMenuOpen() {
        scrollviewAnimator.SetBool("menuOpen", menuOpen);
        panelAnimator.SetBool("menuOpen", menuOpen);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
