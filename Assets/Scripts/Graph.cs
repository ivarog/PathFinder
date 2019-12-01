using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public Node[,] nodes;
    public List<Node> walls = new List<Node>();

    int[,] m_mapData;
    int m_width;
    public int Width {get {return m_width;}}
    int m_height;
    public int Height {get {return m_height;}}


    public static readonly Vector2[] allDirections =
    {
        new Vector2(0f, 1f),
        new Vector2(1f, 1f),
        new Vector2(1f, 0f),
        new Vector2(1f, -1f),
        new Vector2(0f, -1f),
        new Vector2(-1f, -1f),
        new Vector2(-1f, 0f),
        new Vector2(-1f, 1f)
    };

    public void Init(int[,] mapData)
    {
        //Pasamos el Mapa con los datos de las casillas
        m_mapData = mapData;
        //Obtenemos la longitud del mapa
        m_width = mapData.GetLength(0);
        m_height = mapData.GetLength(1);

        //Arreglo de longitud del mapa pero conteniendo a los nodos
        nodes = new Node[m_width, m_height];

        for(int y = 0; y < m_height; y++)
        {
            for(int x = 0; x < m_width; x++)
            {
                //Obtenemos el tipo del nodo atraves de la disponibilidad de la casilla
                NodeType type = (NodeType)mapData[x, y];
                //Creamos un nuevo nodo pasándole el índice del mapa como su índice
                Node newNode = new Node(x, y, type);
                //Agregamos el nodo al arreglo en su índice
                nodes[x, y] = newNode;
                

                newNode.position = new Vector3(x, 0, y);

                //Si esta bloqueado lo agregamos a la lista paredes
                if(type == NodeType.Blocked)
                {
                    walls.Add(newNode);
                }

                //Si no es pared calculamos sus vecinos
                if(nodes[x,y].nodeType != NodeType.Blocked)
                {
                    nodes[x,y].neighbors = GetNeighbors(x,y);
                }
            }
        }
    }

    //Ve rifica si los indices se encuentran dentro de los límites
    public bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < m_width && y >=0 && y < m_height);
    }

    List<Node> GetNeighbors(int x, int y, Node[,] nodeArray, Vector2[] directions)
    {
        List<Node> neighborNodes = new List<Node>();

        //Recorro las direcciones posibles
        foreach(Vector2 dir in directions)
        {
            //Al indice actual le sumo un punto en la direccion nueva
            int newX = x +(int)dir.x;
            int newY = y +(int)dir.y;

            //Si esta dentro de los limites, no es nulo y no esta blockeado entonces se agrega a los vecinos
            if(IsWithinBounds(newX, newY) && nodeArray[newX,newY] != null && nodeArray[newX,newY].nodeType != NodeType.Blocked)
            {
                neighborNodes.Add(nodeArray[newX, newY]);
            }
        }

        return neighborNodes;
    }

    List<Node> GetNeighbors(int x, int y)
    {
        return GetNeighbors(x, y, nodes, allDirections);
    }
}
