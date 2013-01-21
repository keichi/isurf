using System;

namespace IsosurfaceGenerator
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			var reader = new GradsFileReader("pawr3D_20120722-180020.ctl", "pawr3D_20120722-180020.dat");
			reader.ReadData();
			sw.Stop();
			Console.WriteLine("Grads file read -- {0}ms elapsed.", sw.ElapsedMilliseconds);
			sw.Restart();

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
			                           30.0f
			                           );
			sw.Stop();
			Console.WriteLine("mc cube init -- {0}ms elapsed.", sw.ElapsedMilliseconds);
			sw.Restart();

			mc.CalculateIsosurface();
			sw.Stop();
			Console.WriteLine("mc generate isosurface -- {0}ms elapsed.", sw.ElapsedMilliseconds);
		}
	}
}
