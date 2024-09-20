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
        _grid = new GridHex<GridObject>(width: 11, height: 7, 1f, (GridHex<GridObject> g, int x, int y) => new GridObject());
        _camera = Camera.main;

        int[][] testArray = new int[7][];

        testArray[0] = new int[_grid.GetWidth()-3];
        testArray[1] = new int[_grid.GetWidth()-2];
        testArray[2] = new int[_grid.GetWidth()-1];
        testArray[3] = new int[_grid.GetWidth()];
        testArray[4] = new int[_grid.GetWidth()-1];
        testArray[5] = new int[_grid.GetWidth()-2];
        testArray[6] = new int[_grid.GetWidth()-3];

        float xOffset = 0;

        for (int x = 0; x < testArray.Length; x++)
        {
            xOffset = (testArray[3].Length - testArray[x].Length) * 1f * 0.5f;
            for (int y = 0; y < testArray[x].Length; y++)
            {
                Transform cloneTransform = Instantiate(prefabHex, _grid.GetWorldPosition(x: y, y: x, xOffset), Quaternion.identity);
                Debug.Log(xOffset);
                //_grid.GetHexValue(x, y).cloneTransform = cloneTransform;
                //_grid.GetHexValue(x, y).Hide();
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
