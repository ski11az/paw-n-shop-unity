using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchString : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] StoreFlow flow;

    private void OnMouseEnter()
    {
        anim.SetBool("IsMouseHover", true);
    }

    private void OnMouseExit()
    {
        anim.SetBool("IsMouseHover", false);
    }
    private void OnMouseUpAsButton()
    {
        anim.SetTrigger("MouseClicked");
        flow.StartDay();
    }
}
