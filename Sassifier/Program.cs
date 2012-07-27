using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SassAndCoffee.Ruby.Sass;

namespace Sassifier {
	class Program {

		/// <summary>
		/// Creates a standalone wrapper executable for the <see cref="SassAndCoffee"/> library.
		/// <para>See also https://github.com/xpaulbettsx/SassAndCoffee/issues/44 </para>
		/// </summary>
		/// <param name="args">path, iscompressed, list of dependencies (;)</param>
		static void Main(string[] args) {

			// make sure we have enough args
			if (null == args || 0 == args.Count()) {
				throw new ArgumentNullException("You must provide at least a file path to compile");
			}


			// get the args and defaults
			string path = args[0];
			bool compressed = (args.Count() > 1 ? Convert.ToBoolean(args[1]) : false);
			string[] dependencies = (args.Count() >  2 ? args[2].Split(';') : new string[]{} );

			Console.WriteLine("Building file [{0}]", path);
			Console.WriteLine("	as {0}compressed", compressed ? string.Empty : "un");
			Console.WriteLine("	with dependencies [{0}]", string.Join(", ", dependencies));

			// use the compiler
			using (var compiler = new SassCompiler()) {
				var compiled = compiler.Compile(path, compressed, dependencies.ToList());

				// add prefix, etc
				compiled = string.Format("{1}{0}{0}{2}"
					, compressed ? string.Empty : Environment.NewLine
					, compressed ? Properties.Settings.Default.PrefixCompressed : Properties.Settings.Default.Prefix.Replace("{{date}}", DateTime.Now.ToString())
					, compiled
					);

				//write to file
				var destination = string.Format("{1}{0}{2}.css"
					, Path.DirectorySeparatorChar
					, Path.GetDirectoryName(path)
					, Path.GetFileNameWithoutExtension(path)
					);

				File.WriteAllText(destination, compiled);
				Console.WriteLine("SASS Compiled to [{0}]", destination);
			}
		}
	}
}
