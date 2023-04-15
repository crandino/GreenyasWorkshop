using Greenyas.Hexagon;

public class Connection
{
    private Node node;
    private TilePath path;

    public Connection(TilePath path, Node node)
    {
        this.path = path;
        this.node = node;
    }

    public HexSide.Side Side => node.Side;

    public bool IsStarter => path.IsStarter;

    public bool IsFacing(Node againstNode)
    {
        return node.IsFacing(againstNode);
    }

    public Connection[] GoThrough()
    {
        return path.GoThrough(node).Links.ToArray();
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
