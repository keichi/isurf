using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;

using IsosurfaceGenerator.Utils;
using IsosurfaceGenerator.Exporter;

namespace IsosurfaceGenerator
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));

			printCopyrights();

			if (args.Length < 1 || String.IsNullOrWhiteSpace(args[0])) {
				Console.WriteLine("Please specify a GrADS CTL file.");
				return;
			}
			if (args.Length < 2 || String.IsNullOrWhiteSpace(args[1])) {
				Console.WriteLine("Please specify the value you want generate the isosurface.");
				return;
			}
			var ctlFilename = args[0];
			var isoValue = float.Parse(args[1]);

			if (File.Exists(ctlFilename)) {
				processSingleFile(ctlFilename, isoValue);
			} else if(Directory.Exists(ctlFilename)) {
				var files = Directory.GetFiles(ctlFilename, "*.ctl");
				foreach (var file in files) {
					processSingleFile(file, isoValue);
					GC.Collect();
				}
			}
		}

		private static void processSingleFile(string filename, float isoValue) {
			Console.WriteLine ("====================");
			Console.WriteLine ("Start processing CTL file \"{0}\"", filename);

			var processor = new SingleFileProcessor(filename, isoValue, MeshFileType.OBJ);
			processor.Process();
			processor = null;
		}

		private static void printCopyrights() {
			Console.WriteLine("Isosurface Generator {0}\n{1}\n",
			                  Assembly.GetExecutingAssembly().GetName().Version,
			                  ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute))).Copyright
			                  );
		}
	}
}
