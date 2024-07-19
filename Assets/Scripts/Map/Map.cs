using System;
using TestNinsar.ScriptableObjects;
using TestNinsar.Tables;
using UnityEngine;
using UnityEngine.UI;

namespace TestNinsar
{
    //Этого не было в тз, поэтому сделал попроще.
    public class Map : MonoBehaviour
    {
        [SerializeField] private CellOfMap _preafb;
        [SerializeField] private GridLayoutGroup _gridParent;
        [SerializeField] private BootStrap _bootStrap;
        
        private CellOfMap[,] _cells;
        
        private void Awake()
        {
            if (_bootStrap.IsBooted)
            {
                BuildMap();
            }

            _bootStrap.BootEvent += BuildMap;
            _bootStrap.UpdateShapeEvent += UpdateMap;
        }

        private void BuildMap()
        {
            DestroyAllCells();
            var subs = _bootStrap.CurrentInput.Split('\n');
            _gridParent.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _gridParent.constraintCount = subs[0].Length;
            _cells = new CellOfMap[subs[0].Length, subs.Length];
            var converter = new StringToTableConverter<ShapeData>(_bootStrap.Dictionary)
                .GetResult(_bootStrap.CurrentInput);
            for (int y = 0; y < subs.Length; y++)
            {
                for (int x = 0; x < subs[0].Length; x++)
                {
                    _cells[x, y] = Instantiate(_preafb, _gridParent.transform);
                    _cells[x, y].Init(subs[y][x], converter[x, y].Color);
                }
            }
            UpdateMap();
        }

        private void UpdateMap()
        {
            if(_cells== null)
                return;
            
            foreach (var cell in _cells)
            {
                cell.UpdateView(false);
            }

            foreach (var indexes in _bootStrap.Area.GetAreaNormalizedIndexes())
            {
                (int x, int y) = indexes;
                if(_cells.GetLength(0) > x && _cells.GetLength(1) > y)
                    _cells[x, y].UpdateView(true);
            }
        }

        private void DestroyAllCells()
        {
            if(_cells== null)
                return;
            
            foreach (var cell in _cells)
            {
                Destroy(cell.gameObject);
            }
        }
    }
}