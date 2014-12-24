using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometricElements2
{
    public class WeightedPoint
    {
          public WeightedPoint()
                : this(0, 0, 1)
            {
            }

            //copy constructor
            public WeightedPoint(WeightedPoint source)
                : this(source.X, source.Y, source.Weight)
            {
            }

            public WeightedPoint(double X, double Y, double Weight)
            {
                this.X = X;
                this.Y = Y;
                this.Weight = Weight;
            }

            public override bool Equals(object obj)
            {
                WeightedPoint otherPoint = obj as WeightedPoint;
                if (otherPoint == null)
                    throw new ArgumentException(
                        string.Format("Argument must be an instance of {0}. ",
                        typeof(WeightedPoint).FullName));

                return otherPoint.X == X && otherPoint.Y == Y;
            }

            public double X;
            public double Y;
            public double Weight;
        
        }
    }
