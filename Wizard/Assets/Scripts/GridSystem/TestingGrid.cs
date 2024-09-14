using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingGrid : MonoBehaviour
{
    private Grid _grid;
    private Camera _camera;
    private Vector2 _mousePos;

    [SerializeField] Transform pfSquare;

    void Start()
    {
        _grid = new Grid(4, 2, 10f);
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mousePos = GetMouseWorldPos(Input.mousePosition);
            _grid.SetValue(_mousePos, 15);
        }
        if(Input.GetMouseButtonDown(1))
        {
            _mousePos = GetMouseWorldPos(Input.mousePosition);
            Debug.Log(_grid.GetValue(_mousePos));
        }
    }

    private Vector2 GetMouseWorldPos(Vector3 mousePos)
    {
        return _camera.ScreenToWorldPoint(mousePos);
    }

}
