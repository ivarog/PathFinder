using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoController : MonoBehaviour
{
    public MapData mapData;
    public Graph graph;

    public PathFinder pathFinder;
    public int startX = 0;
    public int startY = 0;
    public int goalX = 15;
    public int goalY = 1;

    public float timeStep = 0.1f;

    private void Start() 
    {
        if (mapData != null && graph != null)
        {
            //Obtiene el mapa lógico de los nodos
            int[,] mapInstance = mapData.MakeMap();
            //Crea un grafo a partir del mapa
            graph.Init(mapInstance);

            GraphView graphView = graph.gameObject.GetComponent<GraphView>();

            if(graphView != null)
            {
                //Crea la vista del grafo
                graphView.Init(graph);
            }

            if(graph.IsWithinBounds(startX, startY) && graph.IsWithinBounds(goalX, goalY) && pathFinder != null)
            {
                Node startNode = graph.nodes[startX, startY];
                Debug.Log(startNode);
                Node goalNode = graph.nodes[goalX, goalY];
                pathFinder.Init(graph, graphView, startNode, goalNode);
                StartCoroutine(pathFinder.SearchRoutine(timeStep));
            }
        }    
    }
}
