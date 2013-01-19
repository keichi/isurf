using System;
using System.IO;
using System.Text.RegularExpressions;

namespace IsosurfaceGenerator
{
	public class GradsFileReader
	{
		public float StepX { get; set; }
		public float StepY { get; set; }
		public float StepZ { get; set; }

		public float StartX { get; set; }
		public float StartY { get; set; }
		public float StartZ { get; set; }

		public int SizeX { get; set; }
		public int SizeY { get; set; }
		public int SizeZ { get; set; }

		public float NoDataValue { get; set; }
		
		public float[,,] RawData { get; set; }

		private string _ctlFilename;
		private string _datFilename;
		
		public GradsFileReader (string ctlFilename, string datFilename)
		{
			_ctlFilename = ctlFilename;
			_datFilename = datFilename;
		}

		public void ReadData ()
		{
			// Read contents of CTL file
			using (var reader = new StreamReader(_ctlFilename)) {
				string line;
				var regex = new Regex(@"\s+");
				while ((line = reader.ReadLine()) != null) {
					var cols = regex.Split(line);
					switch (cols[0].ToUpper()) {
					case "XDEF":
						SizeX = int.Parse(cols[1]);
						StartX = float.Parse(cols[3]);
						StepX = float.Parse(cols[4]);
						break;
					case "YDEF":
						SizeY = int.Parse(cols[1]);
						StartY = float.Parse(cols[3]);
						StepY = float.Parse(cols[4]);
						break;
					case "ZDEF":
						SizeZ = int.Parse(cols[1]);
						StartZ = float.Parse(cols[3]);
						StepZ = float.Parse(cols[4]);
						break;
					case "UNDEF":
						NoDataValue = float.Parse(cols[1]);
						break;
					}
				}
			}
						
			// Read contents of DAT file
			RawData = new float[SizeX, SizeY, SizeZ];
			using (var reader = new BinaryReader(File.OpenRead(_datFilename))) {
				for (var z = 0; z < SizeZ; z++) {
					for (var y = 0; y < SizeY; y++) {
						for (var x = 0; x < SizeX; x++) {
							RawData[x, y, z] = reader.ReadSingle();
						}
					}
				}
			}
		}
	}
}
