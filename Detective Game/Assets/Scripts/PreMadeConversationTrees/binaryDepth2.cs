using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class binaryDepth2 : TreeCreationScript
{
    public override void CreateTree() {
        baseTree = CTFL(0);
        ConversationTree b_l = CTFL(1);
        ConversationTree b_r = CTFL(2);
        ConversationTree b_l_l = CTFL(3);
        ConversationTree b_l_r = CTFL(4);
        ConversationTree b_r_l = CTFL(5);
        ConversationTree b_r_r = CTFL(6);
        b_l.addChild(b_l_l);
        b_l.addChild(b_l_r);
        b_r.addChild(b_r_l);
        b_r.addChild(b_r_r);
        baseTree.addChild(b_l);
        baseTree.addChild(b_r);
    }
}
