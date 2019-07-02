using System.Collections;
using System.Collections.Generic;
using System.Text;

public class DebugGatherer 
{
    private string nl = System.Environment.NewLine;
    private StringBuilder debugLinesBuilder = new StringBuilder();
    private bool hasNearestNeighbourDebug = false;
    private bool hasCollisioninfoDebug = false;
    private bool hasTimeInfoDebug = false;
    private bool hasAllCollisionsDebug = false;
    private bool hasNodeItIsInDebug = false;

    public float time = -1;
    public float deltaTime= -1;
    public int n2calculations= -1;
    public int numberOfObjects= -1;
    public float framerate= -1;
    public float nearestNeighbourExecTime = -1;
    public float colCheckExecTime = -1;
    public float buildExecTime = -1;
    public KeyValuePair<AABB, float> nearestNeighbourInfo;

    private void GetFrameStats(Framestats fs)
    {

    }

    public void InitDebug()
    {
        debugLinesBuilder.Clear();
        hasNearestNeighbourDebug = false;
        hasCollisioninfoDebug = false;
        hasTimeInfoDebug = false;
        hasAllCollisionsDebug = false;
        hasNodeItIsInDebug = false;
        time = -1;
        deltaTime= -1;
        n2calculations= -1;
        numberOfObjects= -1;
        framerate= -1;
        nearestNeighbourExecTime = -1;
        colCheckExecTime = -1;
        buildExecTime = -1;
    }

    public void SetFrameInfo(Framestats frameStats)
    {
        time = frameStats.time;
        deltaTime = frameStats.deltaTime;
    }
    
    public void InfoOnNearestNeighbour(KeyValuePair<AABB, float> neigbhourDistance)
    {
        
    }

    public string WholeDebugInfo()
    {
        debugLinesBuilder.Append("Collision system " + GameManager.Instance.ColSys.ColSysName + " is tested" + nl);
        if(time != -1)
            debugLinesBuilder.Append("Debug info was captured at: " + time + " ms" + nl);
        if(deltaTime != -1)
        {
            debugLinesBuilder.Append("Frame was executed in: " + deltaTime + " ms");
            if(framerate != -1)
                debugLinesBuilder.Append(" with framerate of " + framerate + " ms" + nl);
            else
                debugLinesBuilder.Append(nl);
        }
        if(n2calculations != -1)
            debugLinesBuilder.Append("There was " + n2calculations + " hard calculations executed" + nl);
        if(colCheckExecTime != -1)
            debugLinesBuilder.Append("Hard calculations took " + colCheckExecTime + " ms" + nl);
        if(numberOfObjects != -1)
        {
            debugLinesBuilder.Append("Structure created has " + numberOfObjects + " nodes of type" + GameManager.Instance.ColSys.GetRoot().NodeType + nl);
            if(buildExecTime != -1)
                debugLinesBuilder.Append("That was built in : " + buildExecTime + " ms" + nl);
        }
        if(nearestNeighbourExecTime != -1)
        {
            debugLinesBuilder.Append("Nearest neighbour was found in " + nearestNeighbourExecTime + " ms" + nl);
            debugLinesBuilder.Append("Nearest neighbour is " + nearestNeighbourInfo.Key.gameObject.name + " with a distance of " + nearestNeighbourInfo.Value + nl);    
        }

        return debugLinesBuilder.ToString();
    }
}