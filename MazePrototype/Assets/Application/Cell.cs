using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Application
{
    public class Cell : MonoBehaviour
    {
        public GameObject NorthWall;
        public GameObject SouthWall;
        public GameObject EastWall;
        public GameObject WestWall;

        [HideInInspector]
        public int Row{get; set; }
        [HideInInspector]
        public int Column { get; set; }

        [HideInInspector]
        public Cell North { get; set; }
        [HideInInspector]
        public Cell South { get; set; }
        [HideInInspector]
        public Cell East { get; set; }
        [HideInInspector]
        public Cell West { get; set; }

        [HideInInspector]
        public List<Cell> Neighbors
        {
            get { return new[] { North, South, East, West }.Where(c => c != null).ToList(); }
        }

        private Dictionary<Cell, bool> _links;
        public List<Cell> Links()
        {
            return _links.Keys.ToList();
        } 

        void Start()
        {
            _links = new Dictionary<Cell, bool>();
        }

        public void Link(Cell cell, bool bidirectional = true)
        {
            if (_links != null)
            {
                _links[cell] = true;
                if (bidirectional)
                {
                    cell.Link(this, false);
                }
            }
        }

        public void Unlink(Cell cell, bool bidirectional = true)
        {
            _links.Remove(cell);
            if (bidirectional)
            {
                cell.Unlink(this, false);
            }
        }

        public bool Linked(Cell cell)
        {
            if (cell == null)
            {
                return false;
            }
            return _links.ContainsKey(cell);
        }
    }
}
