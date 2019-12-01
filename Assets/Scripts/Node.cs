﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Determina si se puede pasar por el nodo o no
public enum NodeType
{
    Open = 0,
    Blocked = 1
}

public class Node
{
    //Por defecto el nodo esta abierto
    public NodeType nodeType = NodeType.Open;

    public int xIndex = -1;
    public int yIndex = -1;

    //Posición del nodo
    public Vector3 position;

    //Una lista con los nodos vecinos
    public List<Node> neighbors = new List<Node>();

    public Node previous = null;

    //Inicializamos el nodo
    public Node(int xIndex, int yIndex, NodeType nodeType)
    {
        this.xIndex = xIndex;
        this.yIndex = yIndex;
        this.nodeType = nodeType;
    }

    public void Reset()
    {
        previous = null;
    }
}
