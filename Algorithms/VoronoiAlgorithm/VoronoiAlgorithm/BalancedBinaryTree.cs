using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoronoiAlgorithm
{
    public class BalancedBinaryTree<T>
    {
        public BalancedBinaryTree(IComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            _comparer = comparer;
        }

        protected Node<T> _rootNode;
        protected readonly IComparer<T> _comparer;

        protected enum ComparisonResult
        {
            Less,
            Equal,
            Greater
        }

        protected class Node<T>
        {
            protected enum Child
            {
                Greater = 0,
                LessOrEqual = 1
            }

            protected readonly T _item;
            protected readonly Node<T>[] _children;
            protected readonly Node<T> _parent;

            internal T Item
            {
                get
                {
                    return _item;
                }
            }

            public Node(T item, Node<T> parent)
            {
                _item = item;
                _children = new Node<T>[2];
                _parent = parent;
            }

            protected Node<T> GetChild(Child comparisonResult)
            {
                return _children[(int)comparisonResult];
            }

            protected void SetChild(Child comparisonResult, Node<T> node)
            {
                _children[(int)comparisonResult] = node;
            }

            internal Node<T> GetChild(ComparisonResult comparisonResult)
            {
                return GetChild(ChildByComparisonResult(comparisonResult));
            }

            internal void SetChild(ComparisonResult comparisonResult, T item)
            {
                SetChild(
                    ChildByComparisonResult(comparisonResult), 
                    new Node<T>(item, this));
            }

            protected Child ChildByComparisonResult(BalancedBinaryTree<T>.ComparisonResult comparisonResult)
            {
                return comparisonResult == ComparisonResult.Greater? 
                    Child.Greater: Child.LessOrEqual;
            }

            internal void ClearChild(BalancedBinaryTree<T>.ComparisonResult comparisonResult)
            {
                SetChild(ChildByComparisonResult(comparisonResult), null);
            }
        }

        public bool Add(T item)
        {
            if (item == null)
            {
                return false;
            }

            if (_rootNode == null)
            {
                return AddRoot(item);
            }

            Node<T> parentNode = null;
            Node<T> currentNode = _rootNode;

            ComparisonResult comparisonResult;
            do
            {
                comparisonResult = CompareItem(item, currentNode);

                parentNode = currentNode;
                currentNode = currentNode.GetChild(comparisonResult);
            }
            while (currentNode != null);

            parentNode.SetChild(comparisonResult, item);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"> item must not be null</param>
        /// <returns> true iff node has been successfully created and set as root </returns>
        private bool AddRoot(T item)
        {
            _rootNode = new Node<T>(item, parent: null);
            return true;
        }

        public ComparisonResult CompareItem(T externalItem, Node<T> node)
        {
            int comparisonResult = _comparer.Compare(externalItem, node.Item);
            return comparisonResult > 0 ? ComparisonResult.Greater
                : comparisonResult < 0 ? ComparisonResult.Less
                : ComparisonResult.Equal;
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void UnionWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            if (item == null)
            {
                return false;
            }

            if (_rootNode == null)
            {
                return false;
            }

            Node<T> parentNode = null;
            Node<T> currentNode = _rootNode;
            
            var prevComparisonResult = default(ComparisonResult?);
            ComparisonResult currentComparisonResult;
            do
            {
                currentComparisonResult = CompareItem(item, currentNode);

                if (currentComparisonResult == ComparisonResult.Equal)
                {
                    break;
                }

                parentNode = currentNode;
                currentNode = currentNode.GetChild(currentComparisonResult);
                prevComparisonResult = currentComparisonResult;
            }
            while (currentNode != null);

            if (!prevComparisonResult.HasValue)
            {
                Clear();
                return true;
            }
            parentNode.ClearChild(prevComparisonResult.Value);

            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
