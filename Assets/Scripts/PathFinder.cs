using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
 
public class PathFinder : MonoBehaviour
{
    Node m_startNode;
    Node m_goalNode;
    Graph m_graph;
    GraphView m_graphView;

    Queue<Node> m_frontierNodes;
    List<Node> m_exploredNodes;
    List<Node> m_pathNodes;

    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = Color.cyan;
    public Color arrowColor = new Color(0.85f, 0.85f, 0.85f, 1f);
    public Color highlightColor = new Color(1f, 1f, 0.5f, 1f);

    public bool showIterations = true;
    public bool showColors = true;
    public bool showArrows = true;
    public bool exitOnGoal = true;

    public bool isComplete = false;
    int m_iterations = 0;
    
    public void Init(Graph graph, GraphView graphView, Node start, Node goal)
    {
        //Busco si existe el grafo, su vista, nodo inicial y final
        if(start == null || goal == null || graph == null || graphView == null)
        {
            Debug.LogWarning("PATHFINDER Init Error: missing components");
            return;
        }

        if(start.nodeType == NodeType.Blocked || goal.nodeType == NodeType.Blocked)
        {
            Debug.LogWarning("PATHFINDER Init Error: start and goal nodes must be unblocked!");
            return;
        }

        m_graph = graph;
        m_graphView = graphView;
        m_startNode = start;
        m_goalNode = goal;

        ShowColors(graphView, start, goal);

        m_frontierNodes = new Queue<Node>();
        m_frontierNodes.Enqueue(start);
        m_exploredNodes = new List<Node>();
        m_pathNodes = new List<Node>();

        for(int x = 0; x < m_graph.Width; x++)
        {
            for(int y = 0; y < m_graph.Height; y++)
            {
                m_graph.nodes[x, y].Reset();
            }
            
        }

        isComplete = false;
        m_iterations = 0;
    }

    //Colorea nodos clave
    private void ShowColors(GraphView graphView, Node start, Node goal)
    {

        if(graphView == null || start == null || goal == null)
        {
            return;
        }

        if(m_frontierNodes != null)
        {
            graphView.ColorNodes(m_frontierNodes.ToList(), frontierColor);
        }

        if(m_exploredNodes != null)
        {
            graphView.ColorNodes(m_exploredNodes, exploredColor);
        }

        if(m_pathNodes != null && m_pathNodes.Count > 0)
        {
            graphView.ColorNodes(m_pathNodes, pathColor);
        }

        NodeView startNodeView = graphView.nodeViews[start.xIndex, start.yIndex];

        if(startNodeView != null)
        {
            startNodeView.ColorNode(startColor);
        }

        NodeView goalNodeView = graphView.nodeViews[goal.xIndex, goal.yIndex];

        if(goalNodeView != null)
        {
            goalNodeView.ColorNode(goalColor);
        }
    }

    private void ShowColors()
    {
        ShowColors(m_graphView, m_startNode, m_goalNode);
    }

    public IEnumerator SearchRoutine(float timeStep = 0.1f)
    {   
        float timeStart = Time.time;

        yield return null;

        while(!isComplete)
        {
            if(m_frontierNodes.Count > 0)
            {
                Node currentNode = m_frontierNodes.Dequeue();
                m_iterations++;

                if(!m_exploredNodes.Contains(currentNode))
                {
                    m_exploredNodes.Add(currentNode);
                }

                //Maneja la frontera
                ExpandFrontier(currentNode);

                if(m_frontierNodes.Contains(m_goalNode))
                {
                    m_pathNodes = GetPathNodes(m_goalNode);

                    if(exitOnGoal)
                    {
                        isComplete = true;
                    }
                }

                if(showIterations)
                {
                    ShowDiagnostics();
                }

                yield return new WaitForSeconds(timeStep);
            }
            else
            {
                isComplete = true;
            }
        }
        ShowDiagnostics();
        Debug.Log("PATHFINDER SearchRoutine: elapsed time = " + (Time.time - timeStart).ToString() + " seconds");
    }

    private void ShowDiagnostics()
    {
        if(showColors)
        {
            //Colorea los bloques de nuevo
            ShowColors();
        }

        if(m_graphView && showArrows)
        {
            m_graphView.ShowNodeArrows(m_frontierNodes.ToList(), arrowColor);

            if(m_frontierNodes.Contains(m_goalNode))
            {
                m_graphView.ShowNodeArrows(m_pathNodes, highlightColor);
            }
        }
    }

    //Agrega la frontera los nodos
    void ExpandFrontier(Node node)
    {
        if(node != null)
        {
            for(int i = 0; i < node.neighbors.Count; i++)
            {
                //Si el vecino no esta gregado ya
                if(!m_exploredNodes.Contains(node.neighbors[i]) && !m_frontierNodes.Contains(node.neighbors[i]))
                {
                    //Agrego al nodo actual como su nodo previo
                    node.neighbors[i].previous = node;
                    //Los agrego a la lista de frontera
                    m_frontierNodes.Enqueue(node.neighbors[i]);
                }
            }
        }
    }

    List<Node> GetPathNodes(Node endNode)
    {
        List<Node> path = new List<Node>();
        //Regreso la lista vacia
        if(endNode == null)
        {
            return path;
        }
        //Agrego el último nodo como inicio del camino
        path.Add(endNode);

        //El nodo actual es el previo del final
        Node currentNode = endNode.previous;

        //Se repite el mismo proceso hasta llegar al inicio
        while(currentNode != null)
        {
            path.Insert(0, currentNode);
            currentNode = currentNode.previous;
        }

        return path;
    }
}
