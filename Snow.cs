using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capsulajs
{
	internal class Snow
	{
		private static Snowball[] _snow;
		
		static Snow ()
		{
			Console.WriteLine("Gerando flocos de neve...");
			
			_snow = new Snowball[32];

			for(int i = 0; i < _snow.Length; i++)
			{
				_snow[i] = new Snowball();
			}
		}

		public static void TranslateSnow ()
		{
			foreach(Snowball flake in _snow)
			{
				if(flake is null)
				{
					continue;
				}

				Point fpos = flake.GetPosition();
				Tuple<int,int> sz = Utis.GetScreenSize();

				if(fpos.X > sz.Item1 || fpos.Y > sz.Item2)
				{
					flake.ResetPosition();
					fpos = flake.GetPosition();
				}

				int x_sum = 1;

				if(Utis.GetRandomInt(0,16) == 6)
				{
					x_sum = -1;
				}
				
				flake.SetPosition(new Point(fpos.X + x_sum, fpos.Y + 2));
			}
		}

		public static void DestroyAll ()
		{
			foreach(Snowball flake in _snow)
			{
				flake.Destroy();
			}
		}
	}

	class Snowball
	{
		private PictureBox sprite;
		
		public Snowball()
		{
			sprite = new PictureBox()
			{
				Size = new Size(9, 9),
				Location = new Point(0,0),
				BackgroundImage = Utis.LoadImage("./recursos/neve.png"),
				BackgroundImageLayout = ImageLayout.Stretch,
				BackColor = Color.Transparent,
			};

			Form1.GetForm().Controls.Add(sprite);

			ResetPosition(true);
		}

		public void SetPosition (Point pos)
		{
			sprite.Location = pos;
		}

		public Point GetPosition ()
		{
			return sprite.Location;
		}

		public void ResetPosition (bool initial = false)
		{
			Tuple<int,int> sz = Utis.GetScreenSize();

			int x = Utis.GetRandomInt(-sz.Item1,sz.Item1);
			int y = Utis.GetRandomInt(-sz.Item2,-1);

			if(initial)
			{
				y = Utis.GetRandomInt(-sz.Item2,sz.Item2);
			}

			Point neo_pos = new Point(x,y);
			
			sprite.Location = neo_pos;
		}

		public void Destroy ()
		{
			if(sprite.BackgroundImage is null)
			{
				return;
			}

			Console.WriteLine("Destruindo floco.");
			
			Elements.DisposeElement(sprite);
		}
	}
}
