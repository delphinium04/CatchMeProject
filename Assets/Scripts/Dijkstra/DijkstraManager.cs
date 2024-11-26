using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dijkstra.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dijkstra
{
    public class DijkstraManager : MonoBehaviour
    {
        public DijkstraUIManager _dijkstraUIManager;

        Dictionary<NodeEnum, GameObject> _nodeObjects = new();
        List<NodeEnum> _userSelectedNodes;
        NodeEnum[] _bestRoute;

        float _timer = 0;
        WeightedGraph _graph = null;

        #region OUT_VARIABLES

        [Header("Data")] public int _time;
        public int _stageNumber = 1;
        public List<Edge> _edges = new();
        public Transform _nodeParent;
        public Material _lineMaterial;

        [Header("Node")] public NodeEnum _startNode;
        public NodeEnum _endNode;

        #endregion OUT_VARIABLES

       
        void Awake()
        {
            _nodeObjects = new Dictionary<NodeEnum, GameObject>();
        }

        void Start()
        {
            _dijkstraUIManager.OnResetButtonClicked += ResetSelection;
            _dijkstraUIManager.OnConfirmButtonClicked += OnConfirmButtonClicked;
            GameStart();
        }

        void OnDestroy()
        {
            if (_dijkstraUIManager is not null)
            {
                _dijkstraUIManager.OnResetButtonClicked -= ResetSelection;
                _dijkstraUIManager.OnConfirmButtonClicked -= OnConfirmButtonClicked;
            }
        }

        void GameStart()
        {
            InitializeNodes();
            InitializeGraph();
            InitializeRoads();
            RunDijkstraAlgorithm(_graph, _startNode, out var distance, out var tracedVertex);

            _userSelectedNodes = new List<NodeEnum>();
            _bestRoute = tracedVertex[(int)_endNode].ToArray();
            string result = string.Join(" > ", tracedVertex[(int)_endNode].Select(e => ((int)e).ToString()).ToArray());
            Debug.Log($"Best Path: {result}");

            _dijkstraUIManager.SetStage(_stageNumber, (int)_endNode);
            OnNodeClicked(_startNode);
            StartCoroutine(SetTimer(_time));
        }

        void InitializeRoads()
        {
            _graph.Edges.ForEach(e =>
            {
                Transform start = _nodeObjects[e.From].transform;
                Transform destination = _nodeObjects[e.To].transform;

                // Make Line
                GameObject lineGo = new()
                {
                    name = $"{start.name}-{destination.name} Weight Line"
                };
                lineGo.transform.SetParent(start);
                lineGo.transform.localPosition = Vector3.zero;
                LineRenderer line = lineGo.AddComponent<LineRenderer>();
                ConfigureLineRenderer(line, start.position, destination.position);

                // Set Text.text = weight
                GameObject textGo = new GameObject
                {
                    name = $"{start.name}-{destination.name} Weight Text"
                };
                textGo.transform.SetParent(lineGo.transform);
                TextMeshPro tmp = textGo.AddComponent<TextMeshPro>();
                ConfigureTMPText(tmp, e.Weight, (start.position + destination.position) / 2);
            });
        }

        void ConfigureLineRenderer(LineRenderer target, Vector3 startPos, Vector3 endPos)
        {
            target.material = _lineMaterial;
            target.positionCount = 2;
            target.useWorldSpace = true;
            target.startWidth = target.endWidth = 0.5f;
            target.textureMode = LineTextureMode.Tile;
            target.sortingOrder = 5;
            target.SetPosition(0, startPos);
            target.SetPosition(1, endPos);
        }

        void ConfigureTMPText(TextMeshPro target, int weight, Vector3 pos)
        {
            target.text = weight.ToString();
            target.alignment = TextAlignmentOptions.Center;
            target.fontSize = 5;
            target.color = Color.white;
            target.sortingOrder = 10;
            target.transform.position = pos;
        }

        void InitializeNodes()
        {
            int enumCount = Enum.GetValues(typeof(NodeEnum)).Length;
            for (int i = 0; i < enumCount; i++)
            {
                Transform child = _nodeParent.Find(((NodeEnum)i).ToString());
                if (child is not null)
                {
                    _nodeObjects.Add((NodeEnum)i, child.gameObject);
                    child.GetComponent<Node>().OnNodeClicked += OnNodeClicked;
                }
                else
                    Debug.Log($"{(NodeEnum)i}'s Object doesn't exist");
            }
        }

        void InitializeGraph()
        {
            _graph = new WeightedGraph(_nodeParent.childCount, false);
            _graph.AddEdge(_edges.ToArray());
        }

        void RunDijkstraAlgorithm(WeightedGraph graph, NodeEnum start, out int[] distance,
            out List<NodeEnum>[] tracedVertex)
        {
            int length = graph.VertexCount;
            tracedVertex = new List<NodeEnum>[length];
            for (int i = 0; i < length; i++)
                tracedVertex[i] = new List<NodeEnum> { start, };
            var weights = graph.GetWeights();
            distance = weights[(int)start];

            HashSet<NodeEnum> visitedVertex = new HashSet<NodeEnum> { start };

            NodeEnum minVertex = start;
            while (visitedVertex.Count < length)
            {
                NodeEnum minVertexExceptForWhile = minVertex;
                for (int node = 0, min = WeightedGraph.INF; node < length; node++)
                {
                    if (visitedVertex.Contains((NodeEnum)node) || distance[node] > min) continue;
                    min = distance[node];
                    minVertex = (NodeEnum)node;
                }

                if (minVertex == minVertexExceptForWhile)
                {
                    Debug.LogError("No path exists, exit");
                    break;
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

        void ResetSelection()
        {
            _userSelectedNodes.Clear();
            OnNodeClicked(_startNode);
        }

        void OnNodeClicked(NodeEnum node)
        {
            // check validity
            if (_userSelectedNodes.Contains(node))
                return;
            if (_userSelectedNodes.Count > 0 && _userSelectedNodes.Last() == node)
                return;
            if (_userSelectedNodes.Count > 0 && !_edges.Any(e => e.From == _userSelectedNodes.Last() && e.To == node))
                return;

            _userSelectedNodes.Add(node);
            _dijkstraUIManager.SetUserPath(_userSelectedNodes.Select(e => (int)e).ToArray());
        }

        void OnConfirmButtonClicked()
        {
            if (_userSelectedNodes.Last() != _endNode)
            {
                LoadGameOverScene(GameFail.WrongWay);
                return;
            }

            _dijkstraUIManager.StageClear();
        }

        void LoadGameOverScene(GameFail fail)
        {
            PlayerPrefs.SetInt(StaticText.PlayerPrefGameOverSign, (int) fail);
            SceneManager.LoadScene(StaticText.GameOverSceneName);
        }

        IEnumerator SetTimer(float time)
        {
            _timer = time;
            while (_timer > 0)
            {
                _timer -= Time.deltaTime;
                _dijkstraUIManager.SetTimerText(_timer);
                yield return null;
            }

            _timer = 0;
            LoadGameOverScene(GameFail.TimeOver);
        }
    }
}