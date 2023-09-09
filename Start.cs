using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capsulajs
{
	internal class Start
	{
		static Start()
		{
			SetFormAppearance();
		}

		public static void BeginAgain ()
		{
			Console.WriteLine("Iniciando aplicação...");
			Menu.CreateMenu();
		}

		public static void SetFormAppearance ()
		{
			Form1 f = Form1.GetForm();

			f.BackColor = Color.FromArgb(47, 48, 51);
			f.BackgroundImage = Utis.LoadImage("./recursos/logo.png");
			f.BackgroundImageLayout = ImageLayout.Center;

			f.Text = FileG.Config.name;
		}
	}
}
