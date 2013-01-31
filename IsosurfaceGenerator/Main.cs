using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

using IsosurfaceGenerator.Utils;
using IsosurfaceGenerator.Exporter;

namespace IsosurfaceGenerator
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			initializeProgram();
			printCopyrights();

			var ctlFilename = "";
			var isoValue = 0.0f;

			parseCommandLineArgs(args, ref ctlFilename, ref isoValue);

			if (File.Exists(ctlFilename)) {
				processSingleFile(ctlFilename, isoValue);
			} else if(Directory.Exists(ctlFilename)) {
				var files = Directory.GetFiles(ctlFilename, "*.ctl");
				foreach (var file in files) {
					processSingleFile(file, isoValue);
				}
			}
		}

		private static void initializeProgram()
		{
			Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
			AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs e) => {
				Debug.WriteLine(e.ExceptionObject.ToString());
				Environment.Exit(-1);
			};
		}

		private static void processSingleFile(string filename, float isoValue)
		{
			Console.WriteLine ("====================");
			Console.WriteLine ("Start processing CTL file \"{0}\"", filename);

			var processor = new SingleFileProcessor(filename, isoValue, MeshFileType.OBJ);
			processor.Process();
			processor = null;
		}

		private static void parseCommandLineArgs(string[] args, ref string ctlFilename, ref float isoValue)
		{
			if (args.Length < 1 || String.IsNullOrWhiteSpace(args[0])) {
				Console.WriteLine("Please specify a GrADS CTL file.");
				return;
			}
			if (args.Length < 2 || String.IsNullOrWhiteSpace(args[1])) {
				Console.WriteLine("Please specify the value you want generate the isosurface.");
				return;
			}
			ctlFilename = args[0];
			isoValue = float.Parse(args[1]);
		}

		private static void printCopyrights()
		{
			Console.WriteLine("Isosurface Generator {0}\n{1}\n",
			                  Assembly.GetExecutingAssembly().GetName().Version,
			                  ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute))).Copyright
			                  );
		}
	}
}
