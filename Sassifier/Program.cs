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

            Console.OutputEncoding = System.Text.Encoding.UTF8;

			// make sure we have enough args
			if (null == args || 0 == args.Count()) {
				throw new ArgumentNullException("You must provide at least a file path to compile");
			}


			// get the args and defaults
			string path = args[0];
            string outPath = args[1];
			bool compressed = (args.Count() > 2 ? Convert.ToBoolean(args[1]) : false);
			string[] dependencies = (args.Count() >  3 ? args[3].Split(';') : new string[]{} );

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

                // n00b parse path ^^
                string destination = Path.GetDirectoryName(path) + Path.DirectorySeparatorChar;
                string desTmp;
                string[] outPathTmp = outPath.Split('/');
                for (var i = 0; i < outPathTmp.Length; i++)
                {
                    desTmp = null;
                    if (outPathTmp[i] == "..")
                    {
                        destination = string.Format("{0}"
                            , System.IO.Directory.GetParent(Path.GetDirectoryName(destination)));
                    }
                    else
                    {
                        desTmp = string.Format("{0}"
                            , outPathTmp[i]);
                    }
                    destination += desTmp + Path.DirectorySeparatorChar;
                }

				//write to file
                destination = string.Format("{0}{1}.css"
                        , destination
                        , Path.GetFileNameWithoutExtension(path)
                    );

				File.WriteAllText(destination, compiled);
				Console.WriteLine("SASS Compiled to [{0}]", destination);
			}
		}
	}
}
