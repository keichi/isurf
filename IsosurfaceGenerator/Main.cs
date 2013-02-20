/*
 * This file is part of "IsosurfaceGenerator"
 *
 * Copyright (C) 2013 Keichi TAKAHASHI. All Rights Reserved.
 * Please contact Keichi Takahashi <keichi.t@me.com> for further informations.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * 
 */

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
			var isoValues = (float[])Enumerable.Empty<float>();

			// コマンドライン引数を解析する
			parseCommandLineArgs(args, ref ctlPath, ref outputPath, ref isoValues);

			if (File.Exists(ctlPath)) {
				// もしctlPathがファイルへのパスなら
				processSingleFile(ctlPath, outputPath, isoValues);
			} else if(Directory.Exists(ctlPath)) {
				// もしctlPathがディレクトリへのパスなら
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

        /// <summary>
        /// 初期化を行う。<para/>
        /// キャッチされなかった例外ハンドラを追加する。
        /// </summary>
		private static void initializeProgram()
		{
			Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));

			// 現在のAppDomainへ例外ハンドラを追加
			AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs e) => {
                Console.WriteLine("回復不能なエラーが発生しました:");
                Console.WriteLine(e.ExceptionObject);
				Environment.Exit(-1);
			};
		}

        /// <summary>
        /// １つCTLファイルの処理を行う。
        /// </summary>
        /// <param name="filename">CTLファイル名</param>
        /// <param name="outputPath">出力ディレクトリ</param>
        /// <param name="isoValues">等値曲面の値の配列</param>
		private static void processSingleFile(string filename, string outputPath, float[] isoValues)
		{
            Console.WriteLine ("========================================");
			Console.WriteLine ("CTLファイル\"{0}\"の処理を開始します。", filename);

			var processor = new SingleFileProcessor(filename, outputPath, isoValues, MeshFileType.OBJ);
			processor.Process();
			// あまり意味がない
			processor = null;
		}

        /// <summary>
        /// コマンドライン引数を解析する。
        /// </summary>
        /// <param name="args">コマンドライン引数</param>
        /// <param name="ctlPath">CTLファイルのパス</param>
        /// <param name="outputPath">出力ディレクトリ</param>
        /// <param name="isoValues">等値曲面の値の配列</param>
		private static void parseCommandLineArgs(string[] args, ref string ctlPath, ref string outputPath, ref float[] isoValues)
		{
			// コマンドライン引数が足りないとき
			if (args.Length < 3) {
                Console.WriteLine("コマンドライン引数が足りません: ");
                Console.WriteLine("isurf.exe 入力ファイル/ディレクトリ 出力ディレクトリ 値1 [値2] [値3] […]");
                Environment.Exit(-1);
			}
			ctlPath = args[0];
			outputPath = args[1];
            
            try {
            	// 第三引数以降をfloatへパースする
    			isoValues = args.Skip(2).Select(float.Parse).ToArray();
            }
            catch(FormatException ex) {
                Console.WriteLine("等値曲面を生成する値の指定が間違っています:");
                Console.WriteLine(ex.Message);
                Environment.Exit(-1);
            }
		}

        /// <summary>
        /// 著作権情報を表示する。
        /// </summary>
		private static void printCopyrights()
		{
			// AssemblyInfo.csで設定しているバージョン情報などを取得する
			Console.WriteLine("Isosurface Generator {0}\n{1}\n",
			                  Assembly.GetExecutingAssembly().GetName().Version,
			                  ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute))).Copyright
			                  );
		}
	}
}
