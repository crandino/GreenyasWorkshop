using Greenyas.Hexagon;

public class Connection
{
    private Node node;
    public TileSegment Segment { private set; get; }

    public Connection(TileSegment segment, Node node)
    {
        Segment = segment;
        this.node = node;
    }

    public HexSide.Side Side => node.Side;

    public bool IsStarter => Segment.IsStarter;

    public bool IsFacing(Node againstNode)
    {
        return node.IsFacing(againstNode);
    }

    public Connection[] GoThrough()
    {
        return Segment.GoThrough(node).Links.ToArray();
    }

    public void Connect(Connection[] connections, bool bidirectional = true)
    {
        for (int i = 0; i < connections.Length; i++)
        {
            node.Links.Add(connections[i]);
            if (bidirectional)
                connections[i].node.Links.Add(this);
        }
    }

    public void Disconnect()
    {
        Connection[] links = node.Links.ToArray();
        node.Links.Clear();
        links.Disconnect();       
    }
}

public static class ConnectionExtensions
{
    public static void Disconnect(this Connection[] links)
    {
        for (int i = 0; i < links.Length; i++)
        {
            links[i].Disconnect();
        }
    }
}
