using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConversationTree
{
    public string question;
    public AudioClip answer;
    public List<ConversationTree> children;

    public ConversationTree(string question, AudioClip answer) {
        this.question = question;
        this.answer = answer;
        this.children = null;
    }

    public ConversationTree(string question, AudioClip answer, List<ConversationTree> children) {
        this.question = question;
        this.answer = answer;
        this.children = children;
    }

    public string getTreeString(int layer=0) {
        string layerString = "";
        for (int iii = 0; iii < layer; iii++) {
            layerString = layerString + "\t";
        }
        string treeString = layerString + "Question: " + question+"\n";
        if (isLeaf()) {
            return treeString;
        }
        foreach (ConversationTree child in this.children) {
            treeString = treeString + child.getTreeString(layer+1);
        }
        return treeString;
    }

    public int getNoChildren() {
        if(children == null)
        {
            return 0;
        }
        else {
            return children.Count;
        }
    }

    public bool isLeaf() {
        return (getNoChildren() == 0);
    }

    public void addChild(ConversationTree subTree) {
        if (children == null) {
            children = new List<ConversationTree> { };
        } else {
            if (getNoChildren() > 3)
            {
                return;
            }
        }
        children.Add(subTree);
    }
}
