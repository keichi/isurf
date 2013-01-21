using System;

namespace IsosurfaceGenerator
{
	public struct Voxel
	{
		public Vec3[] Points;
		public float[] Values;

		public Voxel (Vec3[] points, float[] values)
		{
			Points = points;
			Values = values;
		}
	}
}

