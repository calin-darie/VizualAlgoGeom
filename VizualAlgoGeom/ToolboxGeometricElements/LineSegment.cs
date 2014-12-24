using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace VizualAlgoGeom
{
    class LineSegment: GeometricElement
    {
        private Point _firstPoint;
        private Point _secondPoint;

        internal Point firstPoint
        {
            get
            {
                return _firstPoint;
            }
            set
            {
                _firstPoint = value;
            }
        }

        internal Point secondPoint
        {
            get
            {
                return _secondPoint;
            }
            set
            {
                _secondPoint = value;
            }
        }


        internal LineSegment(Point p1, Point p2, String name, String groupName, Color color)
            : base(name, groupName, color)
        {
            _firstPoint = p1;
            _secondPoint = p2;
        }


    }
}
