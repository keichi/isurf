using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace IsosurfaceGenerator
{
	/// <summary>
	/// GrADSファイルの読み込み処理を行うクラス
	/// </summary>
	public class GradsFileReader
	{
		/// <summary>
		/// CTLファイルのパス
		/// </summary>
		private string _ctlFilename;
		/// <summary>
		/// DATファイルのパス
		/// </summary>
		private string _datFilename;
		
		/// <summary>
		/// このクラスのオブジェクトを作成する
		/// </summary>
		/// <param name="ctlFilename">読み込むCTLファイルのパス</param>
		public GradsFileReader (string ctlFilename)
		{
			_ctlFilename = ctlFilename;
		}

		/// <summary>
		/// このクラスのオブジェクトを作成する
		/// </summary>
		/// <param name="ctlFilename">読み込むCTLファイルのパス</param>
		/// <param name="datFilename">読み込むDATファイルのパス</param>
		public GradsFileReader (string ctlFilename, string datFilename) : this(ctlFilename)
		{
			_datFilename = datFilename;
		}

		/// <summary>
		/// DATファイルから格子点のデータを読み込む
		/// </summary>
		/// <param name="datFilename">DATファイルのパス</param>
		/// <param name="data">読み込んだデータを格納するPARDataオブジェクト</param>
		private void readDataset(string datFilename, PARData data) {
			// プロパティアクセスによるオーバーヘッドを抑えるため、ローカル変数にキャッシュしておく
			var sizeX = data.SizeX;
			var sizeY = data.SizeY;
			var sizeZ = data.SizeZ;
			// 全ての格子点の数 * 4バイト(floatのサイズ)
			var bufSize = sizeX * sizeY * sizeZ * 4;
			// 格子点における値を格納する配列を確保
			// あえてアンマネージドメモリから確保するのは、古い.NETのバージョンにおいてLOH(Large Object Heap)が
			// フラグメンテーションするのを防ぐため。また若干アロケーションにかかる時間も短縮される
			var rawData = Marshal.AllocHGlobal(bufSize);
			// BinaryReaderは直接IntPtrの指すメモリへ読み込めないため、byte配列を経由する
			var buf = new byte[bufSize];
			// GCへアンマネージドメモリからメモリを確保したことを通知する
			GC.AddMemoryPressure(bufSize);
		
			// データセットを読み込む
			// BinaryReaderはbyte配列への読み込みにしか対応していないので、まずはbyte配列に書き出す
			using (var reader = new BinaryReader(File.OpenRead(_datFilename))) {
				reader.Read(buf, 0, buf.Length);
			}

			// バッファの中身をfloat配列へコピーする
			Marshal.Copy(buf, 0, rawData, buf.Length);
			// GCが働くように
			buf = null;

			data.RawData = rawData;
		}

		/// <summary>
		//	CTLファイル・DATファイルからデータを読み込む
		/// </summary>
		/// <returns>読み込んだデータを格納したPARDataクラス</returns>
		public PARData ReadData ()
		{
			var data = new PARData();

			using (var reader = new StreamReader(_ctlFilename)) {
				string line;

				// 区切り文字に対応する正規表現
				var regex = new Regex(@"\s+");
				while ((line = reader.ReadLine()) != null) {
					var cols = regex.Split(line);
					switch (cols[0].ToUpper()) {
					// X軸の情報
					case "XDEF":
						data.SizeX = int.Parse(cols[1]);
						data.StartX = float.Parse(cols[3]);
						data.StepX = float.Parse(cols[4]);
						break;
					// Y軸の情報
					case "YDEF":
						data.SizeY = int.Parse(cols[1]);
						data.StartY = float.Parse(cols[3]);
						data.StepY = float.Parse(cols[4]);
						break;
					// Z軸の情報
					case "ZDEF":
						data.SizeZ = int.Parse(cols[1]);
						data.StartZ = float.Parse(cols[3]);
						data.StepZ = float.Parse(cols[4]);
						break;
					// データが無い領域の値
					case "UNDEF":
						data.NoDataValue = float.Parse(cols[1]);
						break;
					// データセットのパス
					case "DSET":
						if (!String.IsNullOrEmpty(_datFilename)) break;
						var dset = cols[1];
						// ^で始まる場合は、データセットはctlファイルからの相対パスとなる
						if (dset.StartsWith("^")) {
							dset = dset.Substring(1);
							_datFilename = Path.Combine(Path.GetDirectoryName(_ctlFilename), dset);
						} else {
							_datFilename = dset;
						}
						break;
					}
				}
			}

			// データセットを読み込み
			readDataset(_datFilename, data);

			return data;
		}
	}
}
