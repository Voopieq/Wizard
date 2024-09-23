using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GridHex<TGridObject>
{
    // Grid dimensions
    private int m_width;
    private int m_height;

    private float m_cellSize;

    private const float m_vertical_hex_offset = 0.75f;

    // Grid array
    private TGridObject[][] m_gridArray;

    //Constructor to initialize grid array
    public GridHex(int width, int height, float cellSize, Func<GridHex<TGridObject>, int, int, TGridObject> createGridObject)
    {
        m_width = width;
        m_height = height;
        m_cellSize = cellSize;

        m_gridArray = new TGridObject[m_height][];

        m_gridArray[0] = new TGridObject[m_width-3];
        m_gridArray[1] = new TGridObject[m_width-2];
        m_gridArray[2] = new TGridObject[m_width-1];
        m_gridArray[3] = new TGridObject[m_width];
        m_gridArray[4] = new TGridObject[m_width-1];
        m_gridArray[5] = new TGridObject[m_width-2];
        m_gridArray[6] = new TGridObject[m_width-3];

        // y is height, x is width
        for (int y=0; y<m_gridArray.Length; y++)
        {
            for(int x=0; x < m_gridArray[y].Length; x++)
            {
                m_gridArray[y][x] = createGridObject(this, y, x);
            }
        }


        // DEBUG
       /* Debug.Log("Width: " + m_width + ". Height: " + m_height);

        for(int x = 0; x < m_width; x++)
        {
            for(int y = 0; y < m_height; y++)
            {
                m_debugTextArray[x,y] = CreateWorldText(GetWorldPosition(x, y) + new Vector3(m_cellSize, m_cellSize) * 0.5f, m_gridArray[x, y].ToString(), Color.white, 30);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x, y + 1), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, m_height), GetWorldPosition(m_width, m_height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(m_width, 0), GetWorldPosition(m_width, m_height), Color.white, 100f);

        SetValue(2, 1, 15);*/

    }


    //============================================================
    //                   COORDINATE CONVERSION
    //============================================================

    // Convert local space coordinates to world space coordinates
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, 0) * m_cellSize + 
               new Vector3(0, y, 0) * m_cellSize * m_vertical_hex_offset + 
               (y % 2 == 1 ? new Vector3(1, 0, 0) * m_cellSize * 0.5f : Vector3.zero);
    }

    // Convert local space coordinates to world space coordinates with offset
    public Vector3 GetWorldPosition(int x, int y, float offset)
    {
        return new Vector3(x, 0, 0) * m_cellSize +
               new Vector3(0, y, 0) * m_cellSize * m_vertical_hex_offset +
               new Vector3(1, 0, 0) * offset;
    }

    // Convert world space coordinates to local space coordinates
    private void GetLocalPosition(Vector3 worldCoord, out int x, out int y)
    {
        int roughY = Mathf.RoundToInt(worldCoord.y / m_cellSize / m_vertical_hex_offset);

        if(roughY >=0 && roughY < m_gridArray.Length)
        {
            float xOffset = (m_gridArray[3].Length - m_gridArray[roughY].Length) * 0.5f * m_cellSize;

            int roughX = Mathf.RoundToInt((worldCoord.x - xOffset) / m_cellSize);

            Vector2Int shortestDistance = new Vector2Int(roughX, roughY);

            /*bool odd = roughY % 2 == 1;
            List<Vector2Int> neighbours = new List<Vector2Int>
            {
                shortestDistance + new Vector2Int(-1, 0),
                shortestDistance + new Vector2Int(1, 0),

                shortestDistance + new Vector2Int(odd ? 1 : -1, 1),
                shortestDistance + new Vector2Int(0, 1),

                shortestDistance + new Vector2Int(odd ? 1 : -1, -1),
                shortestDistance + new Vector2Int(0, -1),
            };

            foreach (Vector2Int neighbour in neighbours)
            {
                if(Vector2.Distance(worldCoord, GetWorldPosition(neighbour.x, neighbour.y)) < 
                   Vector2.Distance(worldCoord, GetWorldPosition(shortestDistance.x, shortestDistance.y)))
                {
                    shortestDistance = neighbour;
                }
            }*/
            x = shortestDistance.x;
            y = shortestDistance.y;
        }
        else
        {
            x = -1;
            y = -1;
        }
    }


    //============================================================
    //                   VALUE ASSIGNMENTS
    //============================================================

    // Set grid array value, giving local space coordinates
    public void SetValue(int x, int y, TGridObject value)
    {
        if(x >= 0 && y >= 0 && x <= m_width && y <= m_height)
        {
            m_gridArray[x][y] = value;
        }
        else
        {
            Debug.Log("Invalid position: " +  x + ", " + y);
        }
    }

    // Set grid array value, giving world space coordinates
    public void SetValue(Vector3 worldCoord, TGridObject value)
    {
        int x, y;
        GetLocalPosition(worldCoord, out x, out y);
        SetValue(x, y, value);
    }

    // Get grid array value, giving local space coordinates
    // y is height, x is width
    public TGridObject GetHexValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < m_gridArray[y].Length && y < m_gridArray.Length)
        {
            return m_gridArray[y][x];
        }
        else
        {
            return default(TGridObject);
        }
    }

    // Get grid array value, giving world space coordinates
    public TGridObject GetHexValue(Vector3 worldCoord)
    {
        int x, y;
        GetLocalPosition(worldCoord,out x,out y);
        Debug.Log(x + ", " + y);
        return GetHexValue(x, y);
    }

    //============================================================
    //                   VARIABLE GETTERS
    //============================================================

    public int GetHeight()
    {
        return m_height;
    }

    public int GetWidth()
    {
        return m_width;
    }


    //============================================================
    //                   DEBUG TEXT OUTPUT
    //============================================================

    // Output debug text to grid
    private TextMeshPro CreateWorldText(Vector3 localPosition, string text, Color color, int fontSize, TextAlignmentOptions alignment = TextAlignmentOptions.Center)
    {
        GameObject worldText = new GameObject("Grid World Text", typeof(TextMeshPro));
        Transform transform = worldText.transform;
        transform.localPosition = localPosition;
        TextMeshPro textMeshPro = worldText.GetComponent<TextMeshPro>();
        textMeshPro.text = text;
        textMeshPro.color = color;
        textMeshPro.alignment = alignment;

        return textMeshPro;
    }

}
