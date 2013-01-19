using System;

namespace IsosurfaceGenerator
{
	public class Triangle
	{
		public Vec3 Vertex1 { get; set; }
		public Vec3 Vertex2 { get; set; }
		public Vec3 Vertex3 { get; set; }

		public Triangle (Vec3 v1, Vec3 v2, Vec3 v3)
		{
			Vertex1 = v1;
			Vertex2 = v2;
			Vertex3 = v3;
		}
	}
}

