﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class MapData : MonoBehaviour
{
    //Longitud que tendrá el mapa
    public int width = 10;
    public int height = 5;

    public TextAsset textAsset;
    public Texture2D textureMap;
    public string resourcePath = "MapData";

    private void Start() 
    {
        string levelName = SceneManager.GetActiveScene().name;
        if(textureMap == null)
        {
            textureMap = Resources.Load(resourcePath + "/" + levelName) as Texture2D;
        }
        if(textAsset == null)
        {
            textAsset = Resources.Load( resourcePath + "/" + levelName) as TextAsset;
        }
    }

    //Funcion que obtiene las lineas de un archivo de texto
    public List<string> GetMapFromTextFile(TextAsset tAsset)
    {
        List<string> lines = new List<string>();
        if(tAsset != null)
        {
            string textData = tAsset.text;
            string[] delimiters = {"\r\n", "\n"};
            lines.AddRange(textData.Split(delimiters, System.StringSplitOptions.None)); 
            lines.Reverse();
        }
        else
        {
            Debug.LogWarning("MAPDATA GetMapFromTextFile Error: invalid TextAsset");
        }
        return lines;
    }

    public List<string> GetMapFromTextFile()
    {
        
        return GetMapFromTextFile(textAsset);
    }

    public List<string> GetMapFromTexture(Texture2D texture)
    {
        List<string> lines = new List<string>();

        if(texture != null)
        {
            for(int y = 0; y < texture.height; y++)
            {
                string newLine = "";

                for(int x = 0; x < texture.width; x++)
                {
                    if(texture.GetPixel(x,y) == Color.black)
                    {
                        newLine += '1';
                    }
                    else if(texture.GetPixel(x,y) == Color.white)
                    {
                        newLine += '0';
                    }
                    else
                    {
                        newLine += ' ';
                    }
                }

                lines.Add(newLine);
            }
        }

        return lines;
    }
    //Funcion que ajusta las dimensiones del arreglo
    public void SetDimensions(List<string> textLines)
    {
        height = textLines.Count;

        foreach (string line in textLines)
        {
            if(line.Length > width)
            {
                width = line.Length;
            }
        }
    }

    //Funcion que regresa un arreglo de ods dimensiones con las característica de la casilla (Libre, ocupada)
    public int[,] MakeMap()
    {   
        List<string> lines = new List<string>();
        
        if(textureMap != null)
        {
            lines = GetMapFromTexture(textureMap);
        }
        else
        {
            lines = GetMapFromTextFile(textAsset);
        }
        SetDimensions(lines);

        //Arreglo de dos dimensiones con las longitudes establecidas
        int[,] map = new int[width, height];

        //Rellenamos el arreglo 
        for(int y=0; y<height; y++)
        {
            for(int x=0; x<width; x++)
            {
                if(lines[y].Length > x)
                {
                    map[x, y] = (int)Char.GetNumericValue(lines[y][x]);
                }
            }
        }


        return map;
    }
}
