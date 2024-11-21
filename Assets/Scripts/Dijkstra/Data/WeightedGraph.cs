using System.Collections.Generic;
using System.Linq;
using Dijkstra.Data;

public class WeightedGraph
{
    public const int INF = 20000000;
    public int VertexCount;

    bool Directed = false;
    public List<Edge> Edges;

    public WeightedGraph(int vertexCount, bool directed)
    {
        VertexCount = vertexCount;
        Directed = directed;

        Edges = new List<Edge>();
    }

    public void AddEdge(params Edge[] e)
    {
        List<Edge> edges = e.ToList();
        Edges.AddRange(edges);
        if (!Directed)
            Edges.AddRange(edges.Select(edge => new Edge(edge.To, edge.From, edge.Weight)));
    }

    public List<int[]> GetWeights()
    {
        List<int[]> weights = new List<int[]>();
        for (int i = 0; i < VertexCount; i++)
            weights.Add(Enumerable.Repeat(INF, VertexCount).ToArray());

        Edges.ForEach(e => { weights[(int)e.From][(int)e.To] = e.Weight; });

        for (int i = 0; i < VertexCount; i++)
            weights[i][i] = 0;

        return weights;
    }
}