using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capsulajs
{
	internal class Elements
	{
		public static Button CreateButton (string text, Point pos, Size sz)
		{
			Button b = new Button ()
			{
				Text = text,
				Name = "b_" + text.Trim().ToLower(),
				Size = sz,
				Location = pos,
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.Transparent,
				ForeColor = Color.White,
				Font = FontG.ApplyFont(),
				Cursor = Cursors.Hand,
			};

			b.MouseEnter += (s,e) => b.BackColor = Color.FromArgb(0,0,0,0);
			b.MouseLeave += (s,e) => b.BackColor = Color.Transparent;

			if(FileG.Config.debugMode)
			{
				b.BackColor = Color.DarkRed;
			}

			return b;
		}

		public static Label CreateLabel (string text, Point pos, Size sz)
		{
			Label _label = new Label ()
			{
				Text = text,
				Name = "label_" + text.Split(" ")[0].Trim().ToLower(),
				Size = sz,
				Location = pos,
				TextAlign = ContentAlignment.MiddleCenter,
				BackColor = Color.Transparent,
				ForeColor = Color.White,
				Font = FontG.ApplyFont()
			};

			if(FileG.Config.debugMode)
			{
				_label.BackColor = Color.DarkRed;
			}

			return _label;
		}

		public static TextBox CreateTextbox (string text, Point pos, Size sz)
		{
			TextBox _tbox = new TextBox ()
			{
				Text = text,
				Location = pos,
				Size = sz,
				Font = FontG.ApplyFont(),
				ForeColor = Color.White,
				BackColor = Form1.GetForm().BackColor,
				TextAlign = HorizontalAlignment.Center,
			};

			return _tbox;
		}

		public static RichTextBox CreateRichTextbox (string text, Point pos, Size sz)
		{
			RichTextBox _rtbox = new RichTextBox ()
			{
				Text = text,
				Location = pos,
				Size = sz,
				Font = FontG.ApplyFont(),
				ForeColor = Color.White,
				BackColor = Form1.GetForm().BackColor,
				BorderStyle = BorderStyle.FixedSingle,
			};

			return _rtbox;
		}

		public static CheckBox CreateCheckbox (string text, Point pos, Size sz)
		{
			CheckBox _checkbox = new CheckBox ()
			{
				Text = text,
				Location = pos,
				Size = sz,
				Font = FontG.ApplyFont(),
				ForeColor = Color.White,
				BackColor = Color.Transparent
			};

			return _checkbox;
		}

		/* Dispõe do elemento principal e de seus filhos */
		public static void DisposeElement (Control elem)
		{
			if(elem.Controls.Count > 0)
			{
				foreach(Control child in elem.Controls)
				{
					DisposeElement(child);
				}
			}

			if(elem.BackgroundImage != null)
			{
				elem.BackgroundImage.Dispose();
				elem.BackgroundImage = null;
			}

			string[] _names = FontG.GetFontNames();
			
			if(_names.Contains(elem.Font.Name))
			{
				elem.Font.Dispose();
				elem.Font = null;
			}

			elem.Dispose();
		}
	}
}
