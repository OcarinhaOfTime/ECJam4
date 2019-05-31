using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionUtility : MonoBehaviour {
    public class Line {
        public Vector2 start;
        public Vector2 end;

        public Line(Vector2 start, Vector2 end) {
            this.start = start;
            this.end = end;
        }
    }

    public class Rectangle {
        public Vector2 a;
        public Vector2 b;
        public Vector2 c;
        public Vector2 d;

        public Rectangle(Vector2 a, Vector2 b, Vector2 c, Vector2 d) {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }
    }

    public static bool LineLine(Line a, Line b) {
        float uA = ((b.end.x - b.start.x) * (a.start.y - b.start.y) - (b.end.y - b.start.y) * (a.start.x - b.start.x)) /
            ((b.end.y - b.start.y) * (a.end.x - a.start.x) - (b.end.x - b.start.x) * (a.end.y- a.start.y));
        float uB = ((a.end.x - a.start.x) * (a.start.y - b.start.y) - (a.end.y - a.start.y) * (a.start.x- b.start.x)) / 
            ((b.end.y - b.start.y) * (a.end.x - a.start.x) - (b.end.x - b.start.x) * (a.end.y - a.start.y));

        return uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1;
    }
    public static bool LineLine(Line a, Line b, out Vector2 intersection) {
        float uA = ((b.end.x - b.start.x) * (a.start.y - b.start.y) - (b.end.y - b.start.y) * (a.start.x - b.start.x)) /
            ((b.end.y - b.start.y) * (a.end.x - a.start.x) - (b.end.x - b.start.x) * (a.end.y - a.start.y));
        float uB = ((a.end.x - a.start.x) * (a.start.y - b.start.y) - (a.end.y - a.start.y) * (a.start.x - b.start.x)) /
            ((b.end.y - b.start.y) * (a.end.x - a.start.x) - (b.end.x - b.start.x) * (a.end.y - a.start.y));

        intersection.x = a.start.x + (uA * (a.end.x - a.start.x));
        intersection.y = a.start.y + (uA * (a.end.y - a.start.y));

        return uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1;
    }

}
