using Greenyas.Hexagon;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Link
{
    private Node entry;
    private TilePath path;

    //public HexSide.Side ConnectedSide => entry.Side;

    public Link(TilePath entryPath, Node entryNode)
    {
        path = entryPath;
        entry = entryNode;
    }

    //public bool IsFacing(Node node)
    //{
    //    return entry.IsFacing(node);
    //}

    //public Link[] CurrentLinks => entry.Links.ToArray();

    //public Link[] OppositeLinks
    //{
    //    get
    //    {
    //        Node throughNode = nodes[0] == entry ? nodes[1] : nodes[0];
    //        return throughNode.Links.ToArray();
    //    }
    //}

    //public void CloseLink(params Link[] externalLinks)
    //{
    //    for (int i = 0; i < externalLinks.Length; i++)
    //    {
    //        entry.Links.Add(externalLinks[i]);
    //        externalLinks[i].entry.Links.Add(this);
    //    }
    //}

    public void Disconnect()
    {
        Link[] links = entry.Links.ToArray();
        entry.Links.Clear();

        for (int i = 0; i < links.Length; i++)
        {
            links[i].Disconnect();
        }
    }    
}

public static class LinkExtensions
{
    public static void Disconnect(this Link[] links)
    {
        for (int i = 0; i < links.Length; i++)
        {
            links[i].Disconnect();
        }
    }
}
