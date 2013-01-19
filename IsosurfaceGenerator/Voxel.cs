using System;

namespace IsosurfaceGenerator
{
	public class Voxel
	{
		public Vec3[] Points { get; set; }
		public float[] Values { get; set; }

		public Voxel (Vec3[] points, float[] values)
		{
			Points = points;
			Values = values;
		}
	}
}

