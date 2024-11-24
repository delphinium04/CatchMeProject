using Dijkstra.Data;
using UnityEngine;

namespace Dijkstra
{
    public class Node : MonoBehaviour
    {
        public NodeEnum nodeEnum;
        public void OnMouseDown()
        {
            DijkstraManager.Instance.OnNodeClicked(nodeEnum);
        }
    }
}
