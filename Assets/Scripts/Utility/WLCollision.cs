using UnityEngine;
using System.Collections;

namespace WLCollision
{
    using Point3 = Vector3;

    public class Capsule
    {
        // Construction
        public Capsule( Point3 PointA, Point3 PointB, float Radius )
        {
            m_LineSegment = new LineSegment( PointA, PointB );
            m_Radius = Radius;

            m_SphereCenter = m_LineSegment.GetCenter();
            m_SphereRadius = 0.5f * m_LineSegment.Length() + m_Radius;
        }

        public static Capsule GetZeroCapsule()
        {
            return new Capsule( Point3.zero, Point3.zero, 0.0f );
        }

        public void ReConstruct( Point3 PointA, Point3 PointB, float Radius )
        {
            m_LineSegment.ReConstruct( PointA, PointB );
            m_Radius = Radius;

            m_SphereCenter = m_LineSegment.GetCenter();
            m_SphereRadius = 0.5f * m_LineSegment.Length() + m_Radius;
        }

        public static bool Intersects( Capsule CapsuleA, Capsule CapsuleB )
        {
            if (CapsuleA.m_SphereRadius < Point3.kEpsilon || CapsuleB.m_SphereRadius < Point3.kEpsilon)
            {
                return false;
            }

			if ( Point3.SqrMagnitude(CapsuleA.m_SphereCenter - CapsuleB.m_SphereCenter) <=
                ( CapsuleA.m_SphereRadius + CapsuleB.m_SphereRadius ) * ( CapsuleA.m_SphereRadius + CapsuleB.m_SphereRadius ) )
            {
                return ( ShortestDistance( CapsuleA, CapsuleB ) < Point3.kEpsilon );
            }

            return false;
        }

        public static float ShortestDistance( Capsule CapsuleA, Capsule CapsuleB )
        {
			float fShortestLineSegmentDist = LineSegment.ShortestDistance (CapsuleA.m_LineSegment, CapsuleB.m_LineSegment);
			return Mathf.Max( fShortestLineSegmentDist - ( CapsuleA.m_Radius + CapsuleB.m_Radius ), 0.0f );
        }

        private LineSegment m_LineSegment;
        private float       m_Radius;

        private Point3 m_SphereCenter;
        private float m_SphereRadius;
    }

    public class LineSegment
    {
        public LineSegment( Point3 PointA, Point3 PointB )
        {
            m_PointA = PointA;
            m_PointB = PointB;

            m_vDir = m_PointB - m_PointA;
            m_SquareDistance  = Point3.Dot( m_vDir, m_vDir );
        }

        public void ReConstruct( Point3 PointA, Point3 PointB )
        {
            m_PointA = PointA;
            m_PointB = PointB;

            m_vDir = m_PointB - m_PointA;
            m_SquareDistance = Point3.Dot( m_vDir, m_vDir );
        }

        // Interface
        public float Length()
        {
            return m_vDir.magnitude;
        }

        public Point3 GetCenter()
        {
            return ( m_PointA + m_PointB ) * 0.5f;
        }

        public Point3 NearestPoint( Point3 InPoint )
        {
            Point3 OutPoint = new Point3();
            Point3 vAC = InPoint - m_PointA;

            float f = Point3.Dot( m_vDir, vAC );

            if ( f < 0.0f )
            {
                OutPoint = m_PointA;
            }
            else
            {
                float d = m_vDir.magnitude;

                if ( f > d )
                {
                    OutPoint = m_PointB;
                }
                else
                {
                    f /= d;
                    OutPoint = m_PointA + m_vDir * f;
                }
            }

            return OutPoint;
        }

        public static float ShortestDistance( LineSegment LineA, LineSegment LineB )
        {
            /*
             * [(x2 - x1)^2 + (y2- y1)^2 + (z2 - z1)^2]s - [(x2 - x1)(x4 - x3) + (y2 - y1)(y4 - y3) +(z2 - z1)(z4 - z3)]t
             * = (x1 - x2)(x1 - x3) + (y1 - y2)(y1 - y3) + (y1 - y2)(y1 - y3)
             * -[(x2 - x1)(x4 - x3) + (y2 - y1)(y4 - y3) + (z2 - z1)(z4 - z3)]s + [(x4 - x3)^2 + (y4 - y3)^2 + (z4 - z3)^2]t
             * = (x1 - x3)(x4 - x3) + (y1 - y3)(y4 - y3) + (z1 - z3)(z4 - z3)
             */

            float x1subx3 = LineA.m_PointA.x - LineB.m_PointA.x;
            float y1suby3 = LineA.m_PointA.y - LineB.m_PointA.y;
            float z1subz3 = LineA.m_PointA.z - LineB.m_PointA.z;

            float a = LineA.m_SquareDistance;

            float b = LineA.m_vDir.x * LineB.m_vDir.x +
                      LineA.m_vDir.y * LineB.m_vDir.y +
                      LineA.m_vDir.z * LineB.m_vDir.z;

            float c = -LineA.m_vDir.x * x1subx3 +
                      -LineA.m_vDir.y * y1suby3 +
                      -LineA.m_vDir.z * z1subz3;

            float d = LineA.m_vDir.x * LineB.m_vDir.x +
                      LineA.m_vDir.y * LineB.m_vDir.y +
                      LineA.m_vDir.z * LineB.m_vDir.z;

            float e = LineB.m_SquareDistance;

            float f = x1subx3 * LineB.m_vDir.x +
                      y1suby3 * LineB.m_vDir.y +
                      z1subz3 * LineB.m_vDir.z;

            float denominator = a * e - b * d;
            float t = ( a * f + c * d ) / denominator;
            float s = ( c * e + b * f ) / denominator;

            Point3 FirstPoint  = new Point3();
            Point3 SecondPoint = new Point3();

            if ( t > -Point3.kEpsilon && t < ( 1.0f + Point3.kEpsilon ) && s > -Point3.kEpsilon && s < ( 1.0f + Point3.kEpsilon ) )
            {
                FirstPoint  = LineA.m_PointA + s * LineA.m_vDir;
                SecondPoint = LineB.m_PointA + t * LineB.m_vDir;
            }
            else if ( s < 0.0f )
            {
                FirstPoint  = LineA.m_PointA;
                SecondPoint = LineB.NearestPoint( LineA.m_PointA );
            }
            else if ( s > 1.0f )
            {
                FirstPoint  = LineA.m_PointB;
                SecondPoint = LineB.NearestPoint( LineA.m_PointB );
            }
            else if ( t < 0.0f )
            {
                FirstPoint  = LineA.NearestPoint( LineB.m_PointA );
                SecondPoint = LineB.m_PointA;
            }
            else if ( t > 1.0f )
            {
                FirstPoint  = LineA.NearestPoint( LineB.m_PointB );
                SecondPoint = LineB.m_PointB;
            }

            Point3 vShortest = FirstPoint - SecondPoint;

            return vShortest.magnitude;
        }

        // Internal Data
        private Point3  m_PointA;
        private Point3  m_PointB;

        private Point3 m_vDir;
        private float   m_SquareDistance;
    }

}
