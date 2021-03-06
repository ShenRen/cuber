﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clipr;
using CuberLib;

namespace Cuber
{
    class Program
    {
		// Note: This will probably only work well with OBJ files generated by Pix4D
		// as I have only supported the subset of data types it outputs.
		static void Main(string[] args)
        {
			var opt = CliParser.Parse<Options>(args);

			foreach (string path in opt.Input)
			{
                // Check if we are processing an image or a mesh
                if (Path.GetExtension(path).ToUpper().EndsWith("JPG"))
                {
                    ImageTile tiler = new ImageTile(path, opt.xSize, opt.ySize);
                    tiler.GenerateTiles(opt.OutputPath);
                }
                else
                {
                    CubeManager manager = new CubeManager(path, opt.xSize, opt.ySize, opt.zSize);
                    manager.GenerateCubes(Path.Combine(opt.OutputPath, Path.GetFileNameWithoutExtension(path)));
                }
			}

			Console.WriteLine("Complete");
        }

		
    }

	[ApplicationInfo(Description = "Cuber Options")]
	public class Options
	{
		[NamedArgument('x', "xsize", Action = ParseAction.Store,
			Description = "The number of times to subdivide in the X dimension.  Default 10.")]
		public int xSize { get; set; }

		[NamedArgument('y', "ysize", Action = ParseAction.Store,
			Description = "The number of times to subdivide in the Y dimension.  Default 10.")]
		public int ySize { get; set; }

		[NamedArgument('z', "zsize", Action = ParseAction.Store,
			Description = "The number of times to subdivide in the Z dimension.  Default 10.")]
		public int zSize { get; set; }

		[PositionalArgument(0, MetaVar = "OUT",
			Description = "Output folder")]
		public string OutputPath { get; set; }

		[PositionalArgument(1, MetaVar = "IN",
			NumArgs = 1,
			Constraint = NumArgsConstraint.AtLeast,
			Description = "A list of .obj files to process")]
		public List<string> Input { get; set; }

		public Options()
		{
			xSize = 10;
			ySize = 10;
			zSize = 10;
		}
	}

}
