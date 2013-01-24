using System;
using System.IO;
using System.Reflection;

using IsosurfaceGenerator.Utils;
using IsosurfaceGenerator.Exporter;

namespace IsosurfaceGenerator
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine("Isosurface Generator {0}\n{1}\n",
			                  Assembly.GetExecutingAssembly().GetName().Version,
			                  ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute))).Copyright
			                  );

			var sw = new System.Diagnostics.Stopwatch();
			if (args.Length < 1 || String.IsNullOrWhiteSpace(args[0])) {
				Console.WriteLine("Please specify a GrADS CTL file.");
				return;
			}
			if (args.Length < 2 || String.IsNullOrWhiteSpace(args[1])) {
				Console.WriteLine("Please specify the value you want generate the isosurface.");
				return;
			}
			var ctlFilename = args[0];
			var meshFilename = Path.GetFileNameWithoutExtension(ctlFilename) + ".stl";
			var isoValue = float.Parse(args[1]);

			sw.Start();
			var reader = new GradsFileReader(ctlFilename);
			var data = reader.ReadData();
			sw.Stop();
			Console.WriteLine("[1/4] Read/parse GrADS file \"{1}\". ({0}[ms])", sw.ElapsedMilliseconds, ctlFilename);

			sw.Restart();
			var mc = new MarchingCubes(data, isoValue);
			sw.Stop();
			Console.WriteLine("[2/4] Initializing isosurface generator. ({0}[ms])", sw.ElapsedMilliseconds);

			sw.Restart();
			var mesh = mc.CalculateIsosurface();
			sw.Stop();
			Console.WriteLine("[3/4] Generating isosurface where value is {1}. ({0}[ms])", sw.ElapsedMilliseconds, isoValue);

			sw.Restart();
			var exporter = new STLExporter(meshFilename);
			exporter.Export(mesh);
			Console.WriteLine("[4/4] Writing isosurface mesh data to \"{1}\". ({0}[ms])", sw.ElapsedMilliseconds, meshFilename);

			Console.WriteLine();
			Console.WriteLine("Resulted in {0} triangles.", mesh.Count);
		}
	}
}
