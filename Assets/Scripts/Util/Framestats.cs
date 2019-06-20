using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class Framestats
{
    [XmlAttribute]
    public float time;
    [XmlAttribute]
    public float deltaTime;
    [XmlAttribute]
    public int n2calculations;
    [XmlAttribute]
    public int numberOfObjects;
    [XmlAttribute]
    public float framerate;

    public Framestats(float execTime, float deltatime, int n2calcs, int numofobj)
    {
        time = execTime;
        deltaTime = deltatime;
        n2calculations = n2calcs;
        numberOfObjects = numofobj;
        framerate = 1 / deltatime;
    }

    public Framestats()
    {

    }
}
