using System;
using System.Runtime.InteropServices;

namespace IsosurfaceGenerator.Utils
{
	/// <summary>
	/// 3次元空間内の三角形を表現する構造体
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Triangle
	{
		/// <summary>
		/// 1つ目の頂点
		/// </summary>
		public Vec3 Vertex1;
		/// <summary>
		/// 2つ目の頂点
		/// </summary>
		public Vec3 Vertex2;
		/// <summary>
		/// 3つ目の頂点
		/// </summary>
		public Vec3 Vertex3;

		public Triangle (Vec3 v1, Vec3 v2, Vec3 v3)
		{
			Vertex1 = v1;
			Vertex2 = v2;
			Vertex3 = v3;
		}
	}
}

