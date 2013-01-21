using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace IsosurfaceGenerator
{
	public class GradsFileReader
	{
		private string _ctlFilename;
		private string _datFilename;
		
		public GradsFileReader (string ctlFilename)
		{
			_ctlFilename = ctlFilename;
		}

		public GradsFileReader (string ctlFilename, string datFilename) : this(ctlFilename)
		{
			_datFilename = datFilename;
		}

		private void readDataset(string datFilename, PARData data) {
			var sizeX = data.SizeX;
			var sizeY = data.SizeY;
			var sizeZ = data.SizeZ;
			var rawData = new float[sizeZ][][];
			var buf = new byte[sizeX * sizeY * sizeZ * 4];

			// Read contents of DAT file
			using (var reader = new BinaryReader(File.OpenRead(_datFilename))) {
				reader.Read(buf, 0, buf.Length);
			}

			var offset = 0;
			for (var z = 0; z < sizeZ; z++) {
				rawData[z] = new float[sizeY][];
				for (var y = 0; y < sizeY; y++) {
					rawData[z][y] = new float [sizeX];
					for (var x = 0; x < sizeX; x++) {
						rawData[z][y][x] = BitConverter.ToSingle(buf, offset);
						offset += 4;
					}
				}
			}

			data.RawData = rawData;
		}

		public PARData ReadData ()
		{
			var data = new PARData();

			// Read contents of CTL file
			using (var reader = new StreamReader(_ctlFilename)) {
				string line;
				var regex = new Regex(@"\s+");
				while ((line = reader.ReadLine()) != null) {
					var cols = regex.Split(line);
					switch (cols[0].ToUpper()) {
					case "XDEF":
						data.SizeX = int.Parse(cols[1]);
						data.StartX = float.Parse(cols[3]);
						data.StepX = float.Parse(cols[4]);
						break;
					case "YDEF":
						data.SizeY = int.Parse(cols[1]);
						data.StartY = float.Parse(cols[3]);
						data.StepY = float.Parse(cols[4]);
						break;
					case "ZDEF":
						data.SizeZ = int.Parse(cols[1]);
						data.StartZ = float.Parse(cols[3]);
						data.StepZ = float.Parse(cols[4]);
						break;
					case "UNDEF":
						data.NoDataValue = float.Parse(cols[1]);
						break;
					case "DSET":
						if (!String.IsNullOrEmpty(_datFilename)) break;
						var dset = cols[1];
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

			readDataset(_datFilename, data);

			return data;
		}
	}
}
