using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dijkstra.Data;
using TMPro;
using UnityEngine;
using Utils;

namespace Dijkstra
{
    public class DijkstraManager : Singleton<DijkstraManager>
    {
        public DijkstraUIManager _dijkstraUIManager;
        [Header("Data")] 
        public List<Edge> _edges = null;
        public Transform _nodeParent;
        public Material _lineMaterial;
        
        [Header("Node")]
        public NodeEnum _startNode;
        public NodeEnum _endNode;

        WeightedGraph _graph;
        Dictionary<NodeEnum, GameObject> _nodeObjects = null;

        float timer = 0;

        void Awake()
        {
            _nodeObjects = new Dictionary<NodeEnum, GameObject>();
            _edges ??= new List<Edge>();
        }

        void Start()
        {
            GameStart();
        }

        void GameStart()
        {
            NodeInit();
            GraphInit();
            RoadInit();

            Dijkstra(ref _graph, _startNode, out var distance, out var tracedVertex);
            
            string result = string.Join(" > ", tracedVertex[(int)_endNode].Select(e => ((int)e).ToString()).ToArray());
            Debug.Log($"Best Path: {result}");

            StartCoroutine(SetTimer(30));
        }

        void RoadInit()
        {
            _graph.Edges.ForEach(e =>
            {
                Transform start = GetNodeObject<Transform>(e.From);
                Transform destination = GetNodeObject<Transform>(e.To);

                GameObject lineGo = new GameObject
                {
                    name = $"{start.name}-{destination.name} Line"
                };
                lineGo.transform.SetParent(start);
                lineGo.transform.localPosition = Vector3.zero;

                LineRenderer line = lineGo.AddComponent<LineRenderer>();
                SetLineRendererValues(line);
                line.SetPosition(0, start.position);
                line.SetPosition(1, destination.position);
                line.textureMode = LineTextureMode.Tile;
                line.sortingOrder = 5;

                GameObject textGo = new GameObject
                {
                    name = $"{start.name}-{destination.name} Weight Text"
                };
                textGo.transform.SetParent(lineGo.transform);

                TextMeshPro tmp = textGo.AddComponent<TextMeshPro>();
                textGo.transform.position = (start.position + destination.position) / 2;
                tmp.text = e.Weight.ToString();
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.fontSize = 5;
                tmp.color = Color.white;
                tmp.sortingOrder = 10;
            });
        }

        void SetLineRendererValues(LineRenderer target)
        {
            target.material = _lineMaterial;
            target.positionCount = 2;
            target.useWorldSpace = true;
            target.startWidth = target.endWidth = 0.5f;
        }

        void NodeInit()
        {
            int enumCount = Enum.GetValues(typeof(NodeEnum)).Length;
            for (int i = 0; i < enumCount; i++)
            {
                Transform child = _nodeParent.Find(((NodeEnum)i).ToString());
                if (child is not null)
                    _nodeObjects.Add((NodeEnum)i, child.gameObject);
                else
                    Debug.Log($"{(NodeEnum)i}'s Object doesn't exist");
            }
        }

        void GraphInit()
        {
            _graph = new WeightedGraph(_nodeParent.childCount, false);
            _graph.AddEdge(_edges.ToArray());
        }

        T GetNodeObject<T>(NodeEnum nodeEnum)
        {
            if (_nodeObjects.ContainsKey(nodeEnum))
                return _nodeObjects[nodeEnum].GetComponent<T>();
            Debug.LogError($"{nodeEnum} Object was not found");
            return default(T);
        }

        void Dijkstra(ref WeightedGraph graph, NodeEnum start, out int[] distance,
            out List<NodeEnum>[] tracedVertex)
        {
            int length = graph.VertexCount;
            tracedVertex = new List<NodeEnum>[length];
            for (int i = 0; i < length; i++)
                tracedVertex[i] = new List<NodeEnum> { start, };
            var weights = graph.GetWeights();
            distance = weights[(int)start];

            List<NodeEnum> visitedVertex = new List<NodeEnum> { start };

            NodeEnum minVertex = start;
            while (visitedVertex.Count < length)
            {
                for (int node = 0, min = WeightedGraph.INF; node < length; node++)
                {
                    if (visitedVertex.Contains((NodeEnum)node) || distance[node] > min) continue;
                    min = distance[node];
                    minVertex = (NodeEnum)node;
                }

                visitedVertex.Add(minVertex);

                for (int i = 0; i < length; i++)
                {
                    if (distance[i] < distance[(int)minVertex] + weights[(int)minVertex][i]) continue;
                    distance[i] = distance[(int)minVertex] + weights[(int)minVertex][i];
                    tracedVertex[i].Add(minVertex);
                }
            }
        }

        public void OnNodeClicked(NodeEnum node)
        {
            _dijkstraUIManager.SetUserPath((int)node);
        }

        IEnumerator SetTimer(float time)
        {
            timer = time;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                _dijkstraUIManager.SetTimerText(timer);
                yield return null;
            }

            timer = 0;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}