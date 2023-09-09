using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace capsulajs
{
	internal class Menu
	{
		static Menu ()
		{
			SceneManager.CreateScene("menu_pre");
			SceneManager.CreateScene("menu_capsule");
		}
		
		public static void CreateMenu ()
		{	
			Button load = Elements.CreateButton("Abrir",new Point(10,10),new Size(120,50));
			Button loadLast = Elements.CreateButton("Carregar última",new Point(10,70),new Size(120,50));

			Scene m_pre = SceneManager.GetScene("menu_pre");
			Scene m_capsula = SceneManager.GetScene("menu_capsule");
			m_capsula.DisableScene();

			m_pre.AddToScene(load);
			m_pre.AddToScene(loadLast);

			load.Click += (s, e) =>
			{
				string path = FileG.GetFile();
				
				if(path is null)
				{
					return;
				}

				m_pre.DisableScene();
				m_capsula.EnableScene();

				MenuCapsule(path);
			};

			loadLast.Click += (s, e) =>
			{
				Console.WriteLine(FileG.Config.lastFile);
			};
		}

		public static void MenuCapsule (string xmlpath)
		{
			Scene m_capsula = SceneManager.GetScene("menu_capsule");
			ObfItem item = (ObfItem) FileG.ReadXML(xmlpath, typeof(ObfItem));
			Tuple<int,int> screensz = Utis.GetScreenSize();

			Button exec = Elements.CreateButton("Executar",new Point(10,10),new Size(120,50));
			Button edit = Elements.CreateButton("Editar dados",new Point(10,70),new Size(120,50));
			
			// Nome do pipeline
			Label linename = Elements.CreateLabel(item.name,new Point(screensz.Item1 - 422, 10),new Size(400,40));
			
			linename.TextAlign = ContentAlignment.MiddleRight;
			linename.Font.Dispose();
			linename.Font = FontG.ApplyFont(19);

			// Descrição do pipeline
			Label linedesc = Elements.CreateLabel(item.desc,new Point(screensz.Item1 - 230, 60),new Size(200,250));
			
			linedesc.TextAlign = ContentAlignment.TopLeft;
			linedesc.Text += $"\n\nSaída: {item.outdir}/{item.outname}";

			CheckBox comments = Elements.CreateCheckbox("Permite comentários",new Point(screensz.Item1 - 222, 310),new Size(200,40));

			comments.CheckState = item.noComments ? CheckState.Unchecked : CheckState.Checked;

			CheckBox linebreaks = Elements.CreateCheckbox("Permite quebra de linha",new Point(screensz.Item1 - 222, 340),new Size(200,40));

			m_capsula.AddToScene(exec);
			m_capsula.AddToScene(edit);
			m_capsula.AddToScene(linename);
			m_capsula.AddToScene(linedesc);
			m_capsula.AddToScene(comments);
			m_capsula.AddToScene(linebreaks);

			/*string[] shatter = xmlpath.Split('\\');

			Console.WriteLine(shatter[shatter.Length - 1]);*/

			// Executa procedimentos do pipeline
			exec.Click += (s, e) =>
			{
				//SceneManager.GetScene("menu_pre").EnableScene();
				//m_capsula.DestroyScene();
				Steps.MakeOutFile(item, xmlpath);
			};

			// Salva os dados do pipeline no disco
			edit.Click += (s, e) =>
			{
				SceneManager.CreateScene("menu_edit");

				m_capsula.DisableScene();
				SceneManager.GetScene("menu_edit").EnableScene();

				CreateEditMenu(item);
			};
		}

		public static void CreateEditMenu (ObfItem item)
		{
			Scene m_edit = SceneManager.GetScene("menu_edit");
			Tuple<int,int> screensz = Utis.GetScreenSize();

			Button save = Elements.CreateButton("Salvar dados",new Point(10,10),new Size(120,50));
			Button exit = Elements.CreateButton("Voltar",new Point(10,70),new Size(120,50));

			TextBox name = Elements.CreateTextbox(item.name,new Point(screensz.Item1/2 - 200, 50),new Size(400,40));
			RichTextBox desc = Elements.CreateRichTextbox(item.desc,new Point(screensz.Item1/2 - 200, 80),new Size(400,100));

			m_edit.AddToScene(save);
			m_edit.AddToScene(exit);
			m_edit.AddToScene(name);
			m_edit.AddToScene(desc);

			save.Click += (s, e) =>
			{
				/*ObfItem neoitem = new ObfItem()
				{
					name = item.name,
					desc = item.desc,
					includes = item.includes,
					outdir = item.outdir,
					outname = item.outname,

				};*/
			};

			exit.Click += (s, e) =>
			{
				SceneManager.GetScene("menu_capsule").EnableScene();
				SceneManager.RemoveScene(m_edit.GetSceneName());
			};
		}
	}
}
