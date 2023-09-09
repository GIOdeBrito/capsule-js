using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capsulajs
{
	internal class Utis
	{
		public static Tuple<int,int> GetScreenSize ()
		{
			Form1 f = Form1.GetForm();

			return new Tuple<int,int>(f.Width, f.Height);
		}

		public static int GetRandomInt (int min, int max)
		{
			return new Random().Next(min, max);
		}
		
		public static Bitmap LoadImage (string path)
		{
			if(!File.Exists(path))
			{
				throw new FileNotFoundException("Imagem não encontrada.");
			}

			Bitmap bmp = null;

			using(FileStream stream = new FileStream(path, FileMode.Open))
			{
				bmp = (Bitmap) Image.FromStream(stream);
			}

			return bmp;
		}
	}
}
