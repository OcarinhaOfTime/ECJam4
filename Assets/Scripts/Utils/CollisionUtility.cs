using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionUtility {
    [System.Serializable]
    public struct Line {
        public Vector2 start;
        public Vector2 end;

        public Line(Vector2 start, Vector2 end) {
            this.start = start;
            this.end = end;
        }

        public Vector2 direction {
            get {
                return (end - start).normalized;
            }
        }

        public Vector2 center {
            get {
                return (end + start) * .5f;
            }
        }

        public float magnitude {
            get {
                return (end - start).magnitude;
            }
        }
    }

    [System.Serializable]
    public struct Rectangle {
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

        public Rectangle(BoxCollider2D col) {
            Vector2 p = col.transform.position;
            var size = col.bounds.size * .5f;
            a = p + new Vector2(-size.x, -size.y);
            b = p + new Vector2(-size.x, size.y);
            c = p + new Vector2(size.x, size.y);
            d = p + new Vector2(size.x, -size.y);
        }

        public bool isInside(Vector2 p) {
            return p.x >= a.x && p.x >= a.x && p.x < c.x && p.y < c.y;
        }

        public float size {
            get {
                return (c - a).magnitude;
            }
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

    public static List<Vector2> LineIntersectsRect(Line a, BoxCollider2D r) {
        return LineIntersectsRect(a, new Rectangle(r));
    }

    public static bool LineIntersectsRectTest(Line a, BoxCollider2D r) {
        return LineIntersectsRectTest(a, new Rectangle(r));
    }

    public static bool LineIntersectsRectTest(Line a, Rectangle r) {        
        bool left = LineLine(a, new Line(r.a, r.b));
        bool top = LineLine(a, new Line(r.b, r.c));
        bool right = LineLine(a, new Line(r.c, r.d));
        bool bottom = LineLine(a, new Line(r.d, r.a));
        bool sinside = r.isInside(a.start);
        bool einside = r.isInside(a.end);

        return left || top || right || bottom || sinside || einside;
    }

    public static List<Vector2> LineIntersectsRect(Line a, Rectangle r) {
        Vector2 inter;
        List<Vector2> intersections = new List<Vector2>();

        if (r.isInside(a.start)) {
            intersections.Add(a.start);
        }
        if (r.isInside(a.end)) {
            intersections.Add(a.end);
        }
        if (LineLine(a, new Line(r.a, r.b), out inter)) {
            intersections.Add(inter);
        }
        if(LineLine(a, new Line(r.b, r.c), out inter)){
            intersections.Add(inter);
        }
        if(LineLine(a, new Line(r.c, r.d), out inter)){
            intersections.Add(inter);
        }
        if(LineLine(a, new Line(r.d, r.a), out inter)){
            intersections.Add(inter);
        }

        return intersections;
    }

    public static CollisionUtility.Rectangle Collider2Rect(BoxCollider2D col) {
        Vector2 p = col.transform.position;
        var size = col.bounds.size * .5f;
        return new CollisionUtility.Rectangle(
            p + new Vector2(-size.x, -size.y),
            p + new Vector2(-size.x, size.y),
            p + new Vector2(size.x, size.y),
            p + new Vector2(size.x, -size.y));
    }

    public static Rectangle ToRectange(this BoxCollider2D bc2d) {
        return new Rectangle(bc2d);
    }
}
