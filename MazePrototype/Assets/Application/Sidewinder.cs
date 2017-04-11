using System.Collections.Generic;
using UnityEngine;

namespace Assets.Application
{
    public class Sidewinder : MonoBehaviour, IMazeAlgorithm {

        private IEnumerator<GameObject> _currentCell;
        private List<GameObject> _run = new List<GameObject>();

        public Grid Maze()
        {
            foreach (var row in GetComponent<Grid>().Row)
            {
                var run = new List<GameObject>();

                foreach (GameObject cell in row)
                {
                    run.Add(cell);

                    var atEasternBoundary = cell.GetComponent<Cell>().East == null;

                    var atNorthernBoundary = cell.GetComponent<Cell>().North == null;



                    //var shouldCloseOut = atEasternBoundary || (!atNorthernBoundary && Random.Range(0,2) == 0);
                    var shouldCloseOut = (!atNorthernBoundary && Random.Range(0, 2) == 0);

                    if (shouldCloseOut)
                    {
                        var member = run.Sample();
                        if (member.GetComponent<Cell>().North != null)
                        {
                            member.GetComponent<Cell>().Link(member.GetComponent<Cell>().North);
                            member.GetComponent<Cell>().NorthWall.SetActive(true);
                        }

                        run.Clear();
                    }
                    else
                    {
                        cell.GetComponent<Cell>().Link(cell.GetComponent<Cell>().East);
                        cell.GetComponent<Cell>().EastWall.SetActive(true);
                    }
                }
            }
            return GetComponent<Grid>();
        }

        void Start()
        {
            _currentCell = GetComponent<Grid>().Cells.GetEnumerator();
            Maze();
        }

        public bool Step()
        {
            var last = _currentCell.Current;
            _currentCell.MoveNext();
            var cell = _currentCell.Current;
            if (cell != null)
            {
                if (cell.GetComponent<Cell>().Column == 0)
                {
                    _run = new List<GameObject>();
                }
                _run.Add(cell);

                var atEasternBoundary = cell.GetComponent<Cell>().East == null;
                cell.GetComponent<Cell>().EastWall.SetActive(false);

                var atNorthernBoundary = cell.GetComponent<Cell>().North == null;
                cell.GetComponent<Cell>().NorthWall.SetActive(false);

                var shouldCloseOut = atEasternBoundary || (!atNorthernBoundary && Random.Range(0,2) == 0);

                if (shouldCloseOut)
                {
                    var member = _run.Sample();
                    if (member.GetComponent<Cell>().North != null)
                    {
                        member.GetComponent<Cell>().Link(member.GetComponent<Cell>().North);
                        member.GetComponent<Cell>().NorthWall.SetActive(true);
                    }
                    _run.Clear();
                }
                else
                {
                    cell.GetComponent<Cell>().Link(cell.GetComponent<Cell>().East);
                    cell.GetComponent<Cell>().EastWall.SetActive(true);
                }
            }
            return last != _currentCell.Current;
        }

    }
}
