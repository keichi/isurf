using System;
using System.Runtime.InteropServices;

namespace IsosurfaceGenerator
{
	/// <summary>
	/// A 3-dimensional triangle
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Triangle
	{
		/// <summary>
		/// 1st vertex
		/// </summary>
		/// <value>The vertex1.</value>
		public Vec3 Vertex1;
		/// <summary>
		/// 2nd vertex
		/// </summary>
		/// <value>The vertex2.</value>
		public Vec3 Vertex2;
		/// <summary>
		/// 3rd vertex
		/// </summary>
		/// <value>The vertex3.</value>
		public Vec3 Vertex3;

		public Triangle (Vec3 v1, Vec3 v2, Vec3 v3)
		{
			Vertex1 = v1;
			Vertex2 = v2;
			Vertex3 = v3;
		}
	}
}

