using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capsulajs
{
	internal class FontG
	{
		private static PrivateFontCollection _fonts = new PrivateFontCollection();

		static FontG ()
		{
			LoadFonts();
		}

		public static Font ApplyFont (int sz = 11)
		{
			return new Font(_fonts.Families[0], sz);
		}

		public static string[] GetFontNames ()
		{
			List<string> names = new List<string>();

			foreach(FontFamily f in _fonts.Families)
			{
				names.Add(f.Name);
			}

			return names.ToArray();
		}

		private static void LoadFonts ()
		{
			string[] fontspath =
			{
				@"fonts/acephimere.otf"
			};

			foreach(string fontpath in fontspath)
			{
				_fonts.AddFontFile(fontpath);
			}
		}
	}
}
