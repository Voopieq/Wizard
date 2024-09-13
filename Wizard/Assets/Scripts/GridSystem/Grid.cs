using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Grid
{
    // Grid dimensions
    private int m_width;
    private int m_height;

    private float m_cellSize;

    // Grid array
    private int[,] m_gridArray;
    private TextMeshPro[,] m_debugTextArray;

    //Constructor to initialize grid array
    public Grid(int width, int height, float cellSize)
    {
        m_width = width;
        m_height = height;
        m_cellSize = cellSize;

        m_gridArray = new int[m_width, m_height];
        m_debugTextArray = new TextMeshPro[m_width, m_height];

        Debug.Log("Width: " + m_width + ". Height: " + m_height);

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

        SetValue(2, 1, 15);

    }


    //============================================================
    //                   COORDINATE CONVERSION
    //============================================================

    // Convert local space coordinates to world space coordinates
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * m_cellSize;
    }

    // Convert world space coordinates to local space coordinates
    private void GetLocalPosition(Vector3 worldCoord, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldCoord.x / m_cellSize);
        y = Mathf.FloorToInt(worldCoord.y / m_cellSize);
    }


    //============================================================
    //                   VALUE ASSIGNMENTS
    //============================================================

    // Set grid array value, giving local space coordinates
    public void SetValue(int x, int y, int value)
    {
        if(x >= 0 && y >= 0 && x <= m_width && y <= m_height)
        {
            m_gridArray[x, y] = value;
            m_debugTextArray[x, y].text = m_gridArray[x, y].ToString();
        }
        else
        {
            Debug.Log("Invalid position: " +  x + ", " + y);
        }
    }

    // Set grid array value, giving world space coordinates
    public void SetValue(Vector3 worldCoord, int value)
    {
        int x, y;
        GetLocalPosition(worldCoord, out x, out y);
        SetValue(x, y, value);
    }

    // Get grid array value, giving local space coordinates
    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x <= m_width && y <= m_height)
        {
            return m_gridArray[x, y];
        }
        else
        {
            return -1;
        }
    }

    // Get grid array value, giving world space coordinates
    public int GetValue(Vector3 worldCoord)
    {
        int x, y;
        GetLocalPosition(worldCoord,out x,out y);
        return GetValue(x, y);
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
