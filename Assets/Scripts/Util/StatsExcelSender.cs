using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


public class StatsExcelSender : Singleton_MB<StatsExcelSender>
{
    private List<Framestats> stats = new List<Framestats>();
    public void WriteToFile(String filename)
    {
        var stream = new FileStream(filename, FileMode.Create);
        XmlSerializer xs = new XmlSerializer(typeof(List<Framestats>));
        xs.Serialize(stream, stats);
    }

    public void WriteStat(Framestats fs)
    {
        stats.Add(fs);
    }


}
