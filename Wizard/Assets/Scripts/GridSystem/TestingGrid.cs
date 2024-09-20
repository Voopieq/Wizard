using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingGrid : MonoBehaviour
{
    private GridHex<GridObject> _grid;
    private Camera _camera;
    private Vector3 _mousePos;

    [SerializeField] Transform prefabHex;
    private GridObject _gridObject;

    private class GridObject
    {
        public Transform cloneTransform;

        public void Show()
        {
            cloneTransform.Find("Selected").gameObject.SetActive(true);
        }

        public void Hide()
        {
            cloneTransform.Find("Selected").gameObject.SetActive(false);
        }
    }

    void Start()
    {
        _grid = new GridHex<GridObject>(8, 5, 1f, (GridHex<GridObject> g, int x, int y) => new GridObject());
        _camera = Camera.main;

        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int y = 0; y < _grid.GetHeight(); y++)
            {
                Transform cloneTransform = Instantiate(prefabHex, _grid.GetWorldPosition(x, y), Quaternion.identity);
                _grid.GetHexValue(x, y).cloneTransform = cloneTransform;
                _grid.GetHexValue(x, y).Hide();
            }
        }
    }


    private void Update()
    {
        _mousePos = GetMouseWorldPos(Input.mousePosition);
        if (_gridObject != null && _gridObject != _grid.GetHexValue(_mousePos))
        {
            _gridObject.Hide();
        }

        GridObject newGridObject = _grid.GetHexValue(_mousePos);

        if (newGridObject != null && newGridObject != _gridObject)
        {
            newGridObject.Show();
        }
        _gridObject = newGridObject;

    }

    private Vector3 GetMouseWorldPos(Vector3 mousePos)
    {
        return _camera.ScreenToWorldPoint(mousePos);
    }

}
