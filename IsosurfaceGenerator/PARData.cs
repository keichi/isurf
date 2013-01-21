using System;

namespace IsosurfaceGenerator
{
	/// <summary>
	/// Measure data of the phased array radar
	/// </summary>
	public class PARData
	{
		public float StepX { get; set; }
		public float StepY { get; set; }
		public float StepZ { get; set; }
		
		public float StartX { get; set; }
		public float StartY { get; set; }
		public float StartZ { get; set; }
		
		public int SizeX { get; set; }
		public int SizeY { get; set; }
		public int SizeZ { get; set; }
		
		public float NoDataValue { get; set; }
		
		public float[,,] RawData { get; set; }
		
		public PARData ()
		{
		}
	}
}

