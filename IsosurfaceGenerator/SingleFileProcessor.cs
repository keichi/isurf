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
using System.Collections.Generic;
using System.Diagnostics;

using IsosurfaceGenerator.Exporter;
using IsosurfaceGenerator.Utils;

namespace IsosurfaceGenerator
{
    /// <summary>
    /// メッシュデータファイルの種類を表す列挙体
    /// </summary>
	public enum MeshFileType {
		STL,
		OBJ,
	}

    /// <summary>
    /// Single file processor.
    /// </summary>
	public class SingleFileProcessor
	{
        /// <summary>
        /// メッシュデータのフォーマット
        /// </summary>
		private MeshFileType _meshFileType;
		/// <summary>
        /// CTLファイルのパス
        /// </summary>
        private string _ctlPath;
        /// <summary>
        /// メッシュファイルのパス
        /// </summary>
		private string _meshPath;
        /// <summary>
        /// 等値曲面の値の配列
        /// </summary>
		private float[] _isoValues;
        /// <summary>
        /// メッシュファイルのフォーマットと、拡張子の辞書
        /// </summary>
		private Dictionary<MeshFileType, string> MESH_FILE_EXTENSIONS = new Dictionary<MeshFileType, string>() {
			{MeshFileType.OBJ, ".obj"},
			{MeshFileType.STL, ".stl"},
		};
        /// <summary>
        /// メッシュファイルのフォーマットと、エクスポータのファクトリラムダ関数の辞書
        /// </summary>
		private Dictionary<MeshFileType, Func<string, IMeshExporter>> MESH_EXPORTERS = new Dictionary<MeshFileType, Func<string, IMeshExporter>>() {
			{MeshFileType.OBJ, s => new OBJExporter(s)},
			{MeshFileType.STL, s => new STLExporter(s)},
		};

        /// <summary>
        /// このクラスのオブジェクトを作成する
        /// </summary>
        /// <param name="ctlPath">CTLファイルのパス</param>
        /// <param name="outputPath">出力ディレクトリのパス</param>
        /// <param name="isoValue">等値曲面の値</param>
        /// <param name="meshFileType">メッシュデータの形式</param>
		public SingleFileProcessor (string ctlPath, string outputPath, float isoValue, MeshFileType meshFileType)
			: this(ctlPath, outputPath, new float[] {isoValue}, meshFileType)
		{
		}

        /// <summary>
        /// このクラスのオブジェクトを作成する
        /// </summary>
        /// <param name="ctlPath">CTLファイルのパス</param>
        /// <param name="outputPath">出力ディレクトリのパス</param>
        /// <param name="isoValue">等値曲面の値</param>
        /// <param name="meshFileType">メッシュデータの形式</param>
		public SingleFileProcessor (string ctlPath, string outputPath, float[] isoValues, MeshFileType meshFileType)
		{
			_meshFileType = meshFileType;
			_ctlPath = ctlPath;
			_meshPath = getMeshPath(outputPath, ctlPath);
			_isoValues = isoValues;
			
			// もし出力ディレクトリが存在していなければ作成する
			if (!Directory.Exists(outputPath)) {
				Directory.CreateDirectory(outputPath);
			}
		}

        /// <summary>
        /// 出力メッシュファイルのパスを取得する
        /// </summary>
        /// <returns>The mesh path.</returns>
        /// <param name="outputPath">出力ディレクトリのパス</param>
        /// <param name="ctlFilename">CTLファイルのパス</param>
		private string getMeshPath(string outputPath, string ctlFilename) {
			// 出力ディレクトリに拡張子を除いたCTLファイル名を連結する
			var meshFilename = Path.Combine(
				outputPath,
				Path.GetFileNameWithoutExtension(ctlFilename)
				);
			// さらに現在のメッシュデータ形式の拡張子を連結する
			meshFilename += MESH_FILE_EXTENSIONS[_meshFileType];
			
			return meshFilename;
		}

        /// <summary>
        /// 1つのCTLファイルを処理する
        /// </summary>
		public void Process()
		{
			var sw = new Stopwatch ();
			sw.Start ();
			// GrADSファイルのパーサを初期化する
			var reader = new GradsFileReader(_ctlPath);
			// GrADSファイルをパースし、データを読み込む
			using (var data = reader.ReadData()) {
				sw.Stop ();
				Console.WriteLine(String.Format("GrADSファイルを読み込みました。 ({0}[ms])",
					sw.ElapsedMilliseconds)
				);

				sw.Reset();
				sw.Start();
				// Marching Cubes法エンジンを初期化する
				using (var mc = new MarchingCubes(data)) {
					sw.Stop();
					Console.WriteLine(String.Format("等値曲面生成エンジンを初期化しました。 ({0}[ms])",
						sw.ElapsedMilliseconds)
					);

					// 現在のメッシュデータ形式に体するエクスポータクラスのインスタンスを取得する
					using (var exporter = MESH_EXPORTERS[_meshFileType](_meshPath)) {
						foreach (var isoValue in _isoValues) {
							sw.Reset();
							sw.Start();
							mc.UpdateIsosurfaceValue(isoValue);
							var mesh = mc.CalculateIsosurface();
							sw.Stop();
							Console.WriteLine(String.Format("値{1}について等値曲面を生成しました。 ({0}[ms])",
								sw.ElapsedMilliseconds,
								isoValue)
							);

							sw.Reset();
							sw.Start();
							exporter.Export(mesh, isoValue);
							sw.Stop();
                            Console.WriteLine(String.Format("{1}ポリゴンをファイルに出力した。 ({0}[ms])",
								sw.ElapsedMilliseconds,
								mesh.Count)
							);
						}
					}
                    Console.WriteLine(String.Format("メッシュデータを\"{0}\"に出力しました。", _meshPath));
				}
			}
		}
	}
}

