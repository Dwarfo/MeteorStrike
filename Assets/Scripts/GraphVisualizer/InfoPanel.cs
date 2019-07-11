using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    public Text nodeInfo;
    public void UpdateInfo(VisualNode infonode)
    {
        nodeInfo.text = "Depth is: " + infonode.depth + "\nWidth is " + infonode.width + 
            "\nCurrent position is " + infonode.currentPos + "\nNodetype is " + infonode.nodeType;
        if (infonode.parent != null)
            nodeInfo.text += "\nParent is: " + infonode.parent.gameObject.name;
    }
}
