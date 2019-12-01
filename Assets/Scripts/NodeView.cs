using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeView : MonoBehaviour
{
    public GameObject tile;

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

    
}
