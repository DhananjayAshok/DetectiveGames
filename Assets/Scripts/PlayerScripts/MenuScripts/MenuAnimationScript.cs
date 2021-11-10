using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class MenuAnimationScript : MonoBehaviour
{
    public Animator panelAnimator;
    public Animator scrollviewAnimator;
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

    // Update is called once per frame
    void Update()
    {

    }
}
