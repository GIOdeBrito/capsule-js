using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace capsulajs
{
	internal class FileG
	{
		public static MainConfig Config;
		
		static FileG ()
		{
			Config = (MainConfig) ReadXML("./configura/main.xml", typeof(MainConfig));
		}

		public static string GetFile ()
		{
			using(OpenFileDialog explorer = new OpenFileDialog()
			{
				RestoreDirectory = true,
				Title = "Abrir arquivo",
				Filter = "XML (*.xml)|*.xml|Todos (*.*)|*.*",
			})
			{
				if(explorer.ShowDialog() == DialogResult.Cancel)
				{
					Console.WriteLine("Sem arquivo");
					return null;
				}

				return explorer.FileName;
			}
		}

		public static dynamic ReadXML (string path, Type t)
		{
			dynamic dados = null;
			
			try
			{
				XmlSerializer xml = new XmlSerializer(t);
				
				using(StreamReader sreader = new StreamReader(path))
				{
					dados = xml.Deserialize(sreader);
				}
			}
			catch(IOException ex)
			{
				Console.WriteLine($"Erro ao ler arquivo: {path}\n{ex.Message}");
			}
			catch(InvalidOperationException ex)
			{
				Console.WriteLine($"Erro ao deserializar arquivo: {path}\n{ex.Message}");
			}

			return dados;
		}
	}

	[XmlRoot("G_Main")]
	public class MainConfig
	{
		[XmlElement("ProgramName")]
		public string name;
		[XmlElement("LastOpenedFile")]
		public string lastFile;
		[XmlElement("Debug")]
		public Boolean debugMode;
	}

	[XmlRoot("G_ObfItem")]
	public class ObfItem
	{
		[XmlElement("name")]
		public string name;
		[XmlElement("desc")]
		public string desc;
		[XmlElement("include")]
		public string includes;
		[XmlElement("header")]
		public string header;
		[XmlElement("dir")]
		public string outdir;
		[XmlElement("outname")]
		public string outname;
		[XmlElement("nocomments")]
		public bool noComments;
		[XmlElement("nolinebreak")]
		public string hasLinebreaks;
		[XmlElement("obfuscate")]
		public string obfuscate;
	}
}
