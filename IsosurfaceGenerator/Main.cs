using System;

namespace IsosurfaceGenerator
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			var reader = new GradsFileReader("pawr3D_20120722-180020.ctl");
			var data = reader.ReadData();
			sw.Stop();
			Console.WriteLine("Grads file read -- {0}ms elapsed.", sw.ElapsedMilliseconds);
			sw.Restart();

			var mc = new MarchingCubes(data, 30.0f);
			sw.Stop();
			Console.WriteLine("mc cube init -- {0}ms elapsed.", sw.ElapsedMilliseconds);
			sw.Restart();

			mc.CalculateIsosurface();
			sw.Stop();
			Console.WriteLine("mc generate isosurface -- {0}ms elapsed.", sw.ElapsedMilliseconds);
		}
	}
}
