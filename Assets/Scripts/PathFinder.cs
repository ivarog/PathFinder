﻿using System.Collections;
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
    private void ShowColors(GraphView graphView, Node start, Node goal  )
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

    void ShowColors()
    {
        ShowColors(m_graphView, m_startNode, m_goalNode);
    }

    public IEnumerator SearchRoutine(float timeStep = 0.1f)
    {
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
                //Colorea los bloques de nuevo
                ShowColors();

                yield return new WaitForSeconds(timeStep);
            }
            else
            {
                isComplete = true;
            }
        }
    }

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
}