using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace capsulajs
{
	internal class Steps
	{
		public static void MakeOutFile (ObfItem item, string path)
		{
			string realpath = string.Empty;

			string[] shattered = path.Split('\\');
			for(int i = 0; i < shattered.Length - 1; i++)
			{
				realpath += shattered[i] + "/";
			}
			
			string[] files = SelectFiles(realpath+item.includes);
			string strFinal = string.Empty;

			strFinal += ReadFiles(files, realpath);
			
			if(item.noComments)
			{
				strFinal = RemoveComments(strFinal);
			}

			if(item.header != string.Empty)
			{
				strFinal = ReadHeader(realpath+item.header) + strFinal;
			}

			string finalDir = realpath + item.outdir + "/" + item.outname;

			SaveToDisk(finalDir, strFinal);
		}

		public static string[] SelectFiles (string path)
		{
			List<string> files = new List<string>();

			using(StreamReader reader = new StreamReader(path))
			{
				string line = string.Empty;
				while((line = reader.ReadLine()) != null)
				{
					files.Add(line);
				}
			}

			return files.ToArray();
		}

		private static string ReadFiles(string[] files, string path)
		{
			string final = string.Empty;

			foreach(string file in files)
			{
				string file_path = path+file;
				if(File.Exists(path+file))
				{
					string content = $"// --------------------------------\n";
					content += $"//\tARQUIVO: {file}";
					content += $"\n// --------------------------------\n\n";
					content += File.ReadAllText(file_path);
					final += $"{content}\n\n";
				}
			}

			return final;
		}

		private static string ReadHeader (string headerpath)
		{
			if(File.Exists(headerpath))
			{
				return File.ReadAllText(headerpath);
			}

			return "";
		}

		private static void SaveToDisk (string dir, string content)
		{
			File.WriteAllText(dir, content);
			Console.WriteLine(dir);
		}

		static string RemoveComments (string str)
		{
			// Gostaria de agradecer ao meu mano ChatGPT por me fornecer esta loucura aqui
			return Regex.Replace(str, @"(/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/)|(//.*)", string.Empty, RegexOptions.Multiline);
		}
	}
}
