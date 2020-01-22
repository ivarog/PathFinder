﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeView : MonoBehaviour
{
    public GameObject tile;
    public GameObject arrow;
    Node m_node;

    //Espaciado que habrá entre los planos del mapa
    [Range(0, 0.5f)]
    public float borderSize = 0.15f;

    //Inicializa la vista del nodo
    public void Init(Node node)
    {
        //Si si tiene un plano asignado enonces le pone un nombre y lo colaca en su lugar
        if(tile != null)
        {
            gameObject.name = "Node (" + node.xIndex + "," + node.yIndex + ")";
            gameObject.transform.position = node.position;
            tile.transform.localScale = new Vector3(1f - borderSize, 1f, 1f - borderSize);
            m_node = node;
            EnableObject(arrow, false);
        }
    }

    //Cambia el color del nodo dependiendo si es pared o camino
    void ColorNode(Color color, GameObject go)
    {
        if(go != null)
        {
            Renderer goRenderer = go.GetComponent<Renderer>();

            if(goRenderer != null)
            {
                goRenderer.material.color = color;
            }
        }
    }

    public void ColorNode(Color color)
    {
        ColorNode(color, tile);
    }

    void EnableObject(GameObject go, bool state)
    {
        if(go != null)
        {
            go.SetActive(state);
        }
    }

    public void ShowArrow(Color color)
    {
        if(m_node != null && arrow != null && m_node.previous != null)
        {
            EnableObject(arrow, true); 
            Vector3 dirToPrevious = (m_node.previous.position - m_node.position).normalized;
            arrow.transform.rotation = Quaternion.LookRotation(dirToPrevious);

            Renderer arrowRenderer = arrow.GetComponent<Renderer>();

            if(arrowRenderer != null)
            {
                arrowRenderer.material.color = color;
            }
        }
    }
    
}
