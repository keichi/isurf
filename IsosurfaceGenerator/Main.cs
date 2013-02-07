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
	/// <summary>
	/// プログラムのエントリーポイントの存在するクラス
	/// </summary>
	class MainClass
	{
		/// <summary>
		/// プログラムのエントリーポイント
		/// </summary>
		/// <param name="args">コマンドライン引数</param>
		public static void Main (string[] args)
		{
			initializeProgram();
			printCopyrights();

			var ctlPath = "";
			var outputPath = "";
			var isoValues = new float[] {};

			parseCommandLineArgs(args, ref ctlPath, ref outputPath, ref isoValues);

			if (File.Exists(ctlPath)) {
				processSingleFile(ctlPath, outputPath, isoValues);
			} else if(Directory.Exists(ctlPath)) {
				var files = Directory.GetFiles(ctlPath, "*.ctl");
				foreach (var file in files) {
					processSingleFile(file, outputPath, isoValues);
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

		private static void processSingleFile(string filename, string outputPath, float[] isoValues)
		{
			Console.WriteLine ("====================");
			Console.WriteLine ("CTLファイル\"{0}\"の処理を開始します。", filename);

			var processor = new SingleFileProcessor(filename, outputPath, isoValues, MeshFileType.OBJ);
			processor.Process();
			processor = null;
		}

		private static void parseCommandLineArgs(string[] args, ref string ctlPath, ref string outputPath, ref float[] isoValues)
		{
			if (args.Length < 1 || String.IsNullOrEmpty(args[0])) {
				Console.WriteLine("Please specify a GrADS CTL file.");
				return;
			}
			if (args.Length < 2 || String.IsNullOrEmpty(args[1])) {
				Console.WriteLine("Please specify output path.");
				return;
			}
			if (args.Length < 3 || String.IsNullOrEmpty(args[2])) {
				Console.WriteLine("Please specify the value you want generate the isosurface.");
				return;
			}
			ctlPath = args[0];
			outputPath = args[1];
			isoValues = args.Skip(2).Select(s => float.Parse(s)).ToArray();
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
