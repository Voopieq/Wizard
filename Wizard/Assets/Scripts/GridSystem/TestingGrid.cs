using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class TestingGrid : MonoBehaviour
{
    private GridHex<GridObject> _grid;
    private Camera _camera;
    private Vector3 _mousePos;

    [SerializeField] Transform prefabHex;
    [SerializeField] Transform prefabAttack;
    private GridObject _gridObject;

    [SerializeField, Range(0, 1)] private float _attackAmount = 0.2f;
    [SerializeField, Range(0, 1)] private float _skipAmount = 0.1f;

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
        Generate();
    }

    //================= TESTING RANDOM GENERATION =====================
    private void Generate()
    {
        _grid = new GridHex<GridObject>(width: 11, height: 7, 1f, (GridHex<GridObject> g, int y, int x) => new GridObject());
        _camera = Camera.main;

        int[][] testArray = new int[7][];
        List<Vector2Int> coordinates = new List<Vector2Int>();


        testArray[0] = new int[_grid.GetWidth() - 3];
        testArray[1] = new int[_grid.GetWidth() - 2];
        testArray[2] = new int[_grid.GetWidth() - 1];
        testArray[3] = new int[_grid.GetWidth()];
        testArray[4] = new int[_grid.GetWidth() - 1];
        testArray[5] = new int[_grid.GetWidth() - 2];
        testArray[6] = new int[_grid.GetWidth() - 3];

        float xOffset = 0;
        // Create an empty Hex map and add coordinates to List
        for (int y = 0; y < testArray.Length; y++)
        {
            xOffset = (testArray[3].Length - testArray[y].Length) * 1f * 0.5f;
            for(int x = 0; x < testArray[y].Length; x++)
            {
                coordinates.Add(new Vector2Int(x, y));
/*                Transform cloneTransform = Instantiate(prefabHex, _grid.GetWorldPosition(x, y, xOffset), Quaternion.identity);
                _grid.GetHexValue(x, y).cloneTransform = cloneTransform;
                _grid.GetHexValue(x, y).Hide();*/
            }
        }


        int index = 0;
        // Get the number of tiles for attack
        int _attackCount = Mathf.FloorToInt(coordinates.Count * _attackAmount);
        // Get the number of tiles for regular hex
        // SIDE NOTE: use _skipCount if you want to leave some areas empty. For our situation we need the map
        // to always be filled with blank Hexes, and then replace those Hexes texture by randomly selecting.
        // int _skipCount = Mathf.FloorToInt(coordinates.Count * _skipAmount);
        Random random = new Random(5);

        // Iterate through randomly selected coordinates and instantiate either attack hex or regular hex
        foreach (Vector2Int coordinate in coordinates.OrderBy(t => random.Next()).Take(coordinates.Count))
        {
            bool isAttack = index++ < _attackCount;
            xOffset = (testArray[3].Length - testArray[coordinate.y].Length) * 1f * 0.5f;
            Transform prefab = isAttack ? prefabAttack : prefabHex;
            Transform cloneTransform = Instantiate(prefab, _grid.GetWorldPosition(coordinate.x, coordinate.y, xOffset), Quaternion.identity);
            _grid.GetHexValue(coordinate.x, coordinate.y).cloneTransform = cloneTransform;
            _grid.GetHexValue(coordinate.x, coordinate.y).Hide();

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
