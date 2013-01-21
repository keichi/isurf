using System;

namespace IsosurfaceGenerator
{
	/// <summary>
	/// A 3-dimensional triangle
	/// </summary>
	public class Triangle
	{
		/// <summary>
		/// 1st vertex
		/// </summary>
		/// <value>The vertex1.</value>
		public Vec3 Vertex1 { get; set; }
		/// <summary>
		/// 2nd vertex
		/// </summary>
		/// <value>The vertex2.</value>
		public Vec3 Vertex2 { get; set; }
		/// <summary>
		/// 3rd vertex
		/// </summary>
		/// <value>The vertex3.</value>
		public Vec3 Vertex3 { get; set; }

		public Triangle (Vec3 v1, Vec3 v2, Vec3 v3)
		{
			Vertex1 = v1;
			Vertex2 = v2;
			Vertex3 = v3;
		}
	}
}

