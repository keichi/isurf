using System;

namespace IsosurfaceGenerator
{
	public class Vertex
	{
		public Vec3 Point;
		public float Value;
		public bool IsInside;

		public static Vertex Interpolate(Vertex v1, Vertex v2, float isoValue) {
			if (Math.Abs(isoValue - v1.Value) < 0.00001f) {
				return v1;
			}
			if (Math.Abs(isoValue - v2.Value) < 0.00001f) {
				return v2;
			}
			if (Math.Abs(v1.Value - v2.Value) < 0.00001f) {
				return v1;
			}
			
			var diff = (isoValue -  v1.Value) / (v2.Value - v1.Value);

			var v = new Vertex();
			v.Point = new Vec3(
				v1.Point.X  + (v2.Point.X - v1.Point.X) * diff,
				v1.Point.Y + (v2.Point.Y - v1.Point.Y) * diff,
				v1.Point.Z + (v2.Point.Z - v1.Point.Z) * diff
			);
			v.Value = (v1.Value + v2.Value) * 0.5f;

			return v;
		}
	}
}

