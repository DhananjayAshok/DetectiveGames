using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DutchGirl : TreeCreationScript
{
    public override void CreateTree() {
        baseTree = ConversationTreeFromList(0);
        ConversationTree second = ConversationTreeFromList(1);
        ConversationTree third = ConversationTreeFromList(2);
        second.addChild(third);
        baseTree.addChild(second);
        //Debug.Log("Is Triggered");
        //Debug.Log(baseTree);
    }
}
