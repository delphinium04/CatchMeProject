namespace Dijkstra.Data
{
    [System.Serializable]
    public struct Edge
    {
        public NodeEnum From;
        public NodeEnum To;
        public int Weight;

        public Edge(NodeEnum from, NodeEnum to, int weight)
        {
            From = from;
            To = to;
            Weight = weight;
        }
    }

    public enum NodeEnum
    {
        Node0,
        Node1,
        Node2,
        Node3,
        Node4,
        Node5,
        Node6,
        Node7,
        Node8
    }
}