using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace capsulajs
{
	internal class Steps
	{
		public static void MakeOutFile (ObfItem item, string path)
		{
			// String limpa para ficar utilizável com outros arquivos
			string realpath = string.Empty;

			string[] shattered = path.Split('\\');
			for(int i = 0; i < shattered.Length - 1; i++)
			{
				realpath += shattered[i] + "/";
			}
			
			// Pega os arquivos do includes
			string[] files = SelectFiles(realpath, item.includes);
			// Lê o conteúdo dos arquivos e os joga nesta string
			string strFinal = ReadFiles(files, realpath);
			
			if(!item.comments)
			{
				strFinal = RemoveComments(strFinal);
			}

			if(!String.IsNullOrEmpty(item.header))
			{
				strFinal = ReadHeader(realpath+item.header) + strFinal;
			}

			SaveToDisk(realpath, item.outdir, item.outname, strFinal);
		}

		public static string[] SelectFiles (string path, string arqs)
		{
			List<string> files = new List<string>();

			using(StreamReader reader = new StreamReader(path + arqs))
			{
				string line = string.Empty;
				while((line = reader.ReadLine()) != null)
				{
					// Se a linha for vazia, pula para a próxima
					if(String.IsNullOrEmpty(line))
					{
						continue;
					}

                    files.Add(line);
				}
			}

			return files.ToArray();
		}

		private static string ReadFiles(string[] files, string path)
		{
			string final = string.Empty;

            foreach (string file in files)
			{
				string file_path = file;

                /* Verifica se o primeiro caractere da string 
				é uma letra. Se for, usa o caminho completo, do
				contrário usa o caminho relativo até o arquivo */
                if(!Char.IsLetter(file[0]))
                {
					file_path = path+file;
					//Console.WriteLine("Não é absoluto.");
                }

                if(File.Exists(file_path))
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

		private static void SaveToDisk (string realdir, string dir, string name, string content)
		{
			// Caminho até o arquivo que será gravado no disco
			string finalDir = dir;

			// Se não for caminho absoluto
            if(!Char.IsLetter(dir[0]))
            {
                finalDir = realdir + "/" + dir;
            }

			// Adiciona o nome do arquivo no final
			finalDir += "/" + name;

            File.WriteAllText(finalDir, content);
			Console.WriteLine(finalDir);
		}

		static string RemoveComments (string str)
		{
			// Gostaria de agradecer ao meu mano ChatGPT por me fornecer esta loucura aqui
			return Regex.Replace(str, @"(/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/)|(//.*)", string.Empty, RegexOptions.Multiline);
		}
	}
}
