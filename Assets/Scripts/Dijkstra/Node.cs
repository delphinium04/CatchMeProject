using Dijkstra.Data;
using UnityEngine;

namespace Dijkstra
{
    public class Node : MonoBehaviour
    {
        public NodeEnum nodeEnum;
        public void OnMouseDown()
        {
            Debug.Log($"{nodeEnum} clicked");
            TestManager.Instance.OnNodeClicked(nodeEnum);
        }
    }
}
