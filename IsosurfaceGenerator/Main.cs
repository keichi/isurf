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
                    try {
					    processSingleFile(file, outputPath, isoValues);
                    }
                    catch (Exception ex) {
                        Console.WriteLine("ファイル\"{0}\"の処理中にエラーが発生しました:", file);
                        Console.WriteLine(ex.Message);
                    }
				}
			}
		}

		private static void initializeProgram()
		{
			Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
			AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs e) => {
                Console.WriteLine("回復不能なエラーが発生しました:");
                Console.WriteLine(e.ExceptionObject);
				Environment.Exit(-1);
			};
		}

		private static void processSingleFile(string filename, string outputPath, float[] isoValues)
		{
            Console.WriteLine ("========================================");
			Console.WriteLine ("CTLファイル\"{0}\"の処理を開始します。", filename);

			var processor = new SingleFileProcessor(filename, outputPath, isoValues, MeshFileType.OBJ);
			processor.Process();
			processor = null;
		}

		private static void parseCommandLineArgs(string[] args, ref string ctlPath, ref string outputPath, ref float[] isoValues)
		{
			if (args.Length < 3) {
                Console.WriteLine("コマンドライン引数が足りません: ");
                Console.WriteLine("isurf.exe 入力ファイル/ディレクトリ 出力ディレクトリ 値1 [値2] [値3] […]");
                Environment.Exit(-1);
			}
			ctlPath = args[0];
			outputPath = args[1];

            try {
    			isoValues = args.Skip(2).Select(s => float.Parse(s)).ToArray();
            }
            catch(FormatException ex) {
                Console.WriteLine("等値曲面を生成する値の指定が間違っています:");
                Console.WriteLine(ex.Message);
                Environment.Exit(-1);
            }
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
