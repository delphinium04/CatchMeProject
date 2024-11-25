using System;
using Dijkstra.Data;
using UnityEngine;

namespace Dijkstra
{
    public class Node : MonoBehaviour
    {
        public Action<NodeEnum> OnNodeClicked;
        public NodeEnum _nodeEnum;
        public void OnMouseDown()
        {
            OnNodeClicked?.Invoke(_nodeEnum);
        }
    }
}
