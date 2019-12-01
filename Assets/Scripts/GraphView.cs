using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Graph))]
public class GraphView : MonoBehaviour
{
    public GameObject nodeViewPrefab;
    public NodeView[,] nodeViews;

    public Color baseColor = Color.white;
    public Color wallColor = Color.black;

    //Inicializa la vista del grafo
    public void Init(Graph graph)
    {
        if(graph == null)
        {
            Debug.LogWarning("GRAPHView No graph to initialize!");
            return;
        }

        nodeViews = new NodeView[graph.Width, graph.Height];

        //Recorre todos los nodos del grafo
        foreach(Node n in graph.nodes)
        {
            GameObject instance = Instantiate(nodeViewPrefab, Vector3.zero, Quaternion.identity);
            NodeView nodeView = instance.GetComponent<NodeView>();

            if(nodeView != null)
            {
                //Cambia el color dependiendo del tipo
                nodeView.Init(n);
                nodeViews[n.xIndex, n.yIndex] = nodeView;   

                if(n.nodeType == NodeType.Blocked)
                {
                    nodeView.ColorNode(wallColor);
                }
                else
                {
                    nodeView.ColorNode(baseColor);
                }
            }
        }
    }

    //Colorea los nodos provenientes de una lista
    public void ColorNodes(List<Node> nodes, Color color)
    {
        foreach (Node n in nodes)
        {
            if(n != null)
            {
                NodeView nodeView = nodeViews[n.xIndex, n.yIndex];
                if(nodeView != null)
                {
                    nodeView.ColorNode(color);
                }
            }
        }
    }
}
