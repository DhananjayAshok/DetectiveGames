using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class CommonLayers : TreeCreationScript
{
    public int n_layers;
    public int n_options;

    public void CreateLayeredTree(int n_layers, int no_options)
    {
        if (n_layers == 0 || no_options == 0) {
            baseTree = CTFL(0);
            return;
        }
        ConversationTree[] trees = new ConversationTree[1 + (n_layers * no_options)];

        for (int iii = 0; iii < n_layers * no_options+1; iii++) {
            trees[iii] = CTFL(iii);
        }

        // Now trees are all made
        for (int current_layer = n_layers; current_layer >= 0; current_layer --) {
            ArraySegment<ConversationTree> layer = getLayer(trees, current_layer, no_options);
            if (current_layer != 0) {
                ArraySegment<ConversationTree> top_layer = getLayer(trees, current_layer - 1, no_options);
                foreach (ConversationTree topTree in top_layer) {
                    foreach (ConversationTree tree in layer) {
                        topTree.addChild(tree);
                    }
                }
            }
        }
        baseTree = trees[0];
        //Debug.Log(baseTree.getTreeString());
        return;
    }

    ArraySegment<ConversationTree> getLayer(ConversationTree[] trees, int layer_no, int no_options) {
        int start = 0;
        int end = 0;
        if (layer_no == 0) {
            ArraySegment<ConversationTree> small_segment = new ArraySegment<ConversationTree>(trees, 0, 1);
            return small_segment;
        }
        end = no_options * layer_no;
        start = end - no_options + 1;
        ArraySegment<ConversationTree> segment =  new ArraySegment<ConversationTree>(trees, start, no_options);
        return segment;
    }

    public override void CreateTree()
    {
        CreateLayeredTree(n_layers, n_options);
    }

}
