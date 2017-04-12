using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Text;

namespace Assets.Application
{
    public class Grid : MonoBehaviour
    {
        public int Rows;
        public int Columns;
        public GameObject CellGameObject;

        public int Size()
        {
            return Rows * Columns;
        }

        private List<List<GameObject>> _grid;


        private void PrepareGrid()
        {
            _grid = new List<List<GameObject>>();
            for (var r = 0; r < Rows; r++)
            {
                var row = new List<GameObject>();
                for (var c = 0; c < Columns; c++)
                {
                    GameObject g = Instantiate(CellGameObject, new Vector3(r, 0, c), Quaternion.identity);
                    g.GetComponent<Cell>().Row = r;
                    g.GetComponent<Cell>().Column = c;
                    row.Add(g);
                }
                _grid.Add(row);
            }
        }

        private void ConfigureCells()
        {
            foreach (GameObject cell in Cells)
            {
                int row = cell.GetComponent<Cell>().Row;
                int col = cell.GetComponent<Cell>().Column;


                GameObject northGameObject = this[row - 1, col];
                if (northGameObject != null)
                {
                    cell.GetComponent<Cell>().North = northGameObject.GetComponent<Cell>();
                }
                GameObject southGameObject = this[row + 1, col];
                if (southGameObject != null)
                {
                    cell.GetComponent<Cell>().South = southGameObject.GetComponent<Cell>();
                }
                GameObject westGameObject = this[row, col - 1];
                if (westGameObject != null)
                {
                    cell.GetComponent<Cell>().West = westGameObject.GetComponent<Cell>();
                }

                GameObject eastGameObject = this[row, col + 1];
                if (eastGameObject != null)
                {
                    cell.GetComponent<Cell>().East = eastGameObject.GetComponent<Cell>();
                }
            }
        }

        [CanBeNull]
        public virtual GameObject this[int row, int column]
        {
            get
            {
                if (row < 0 || row >= Rows)
                {
                    return null;
                }
                if (column < 0 || column >= Columns)
                {
                    return null;
                }
                return _grid[row][column];
            }
        }

        [NotNull]
        public GameObject RandomCell()
        {
            var row = Random.Range(0, Rows);
            var col = Random.Range(0, Columns);
            var randomCell = this[row, col];
            if (randomCell == null)
            {
                throw new InvalidOperationException("Random cell is null");
            }
            return randomCell;
        }

        public IEnumerable<List<GameObject>> Row
        {
            get
            {
                foreach (var row in _grid)
                {
                    yield return row;
                }
            }
        }

        public IEnumerable<GameObject> Cells
        {
            get
            {
                foreach (var row in Row)
                {
                    foreach (var cell in row)
                    {
                        yield return cell;
                    }
                }
            }
        }

        void Awake()
        {
            PrepareGrid();
            ConfigureCells();
        }

    }
}
