using System;

namespace IsosurfaceGenerator
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine("Input data reading ...");
			var reader = new GradsFileReader("pawr3D_20120722-180020.ctl", "pawr3D_20120722-180020.dat");
			reader.ReadData();
			Console.WriteLine("Successfully read !");

			var mc = new MarchingCubes(reader.StartX,
			                           reader.StartY,
			                           reader.StartZ,
			                           reader.StepX,
			                           reader.StepY,
			                           reader.StepZ,
			                           reader.SizeX,
			                           reader.SizeY,
			                           reader.SizeZ,
			                           reader.RawData,
			                           45.0f
			                           );

			Console.WriteLine("Calculating iso surface ...");
			mc.CalculateIsosurface();
			Console.WriteLine("Successfully calculated !");
		}
	}
}
