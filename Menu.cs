using System;
using System.Collections.Generic;
using System.IO;
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

				// Atualiza a configuração da aplicação
				FileG.WriteXML("configura/main.xml",new MainConfig()
				{
					name = FileG.Config.name,
					lastFile = path,
					debugMode = FileG.Config.debugMode,
				});

				MenuCapsule(path);
			};

			loadLast.Click += (s, e) =>
			{
				if(String.IsNullOrEmpty(FileG.Config.lastFile))
				{
					return;
				}
				
				MenuCapsule(FileG.Config.lastFile);
			};
		}

		public static void MenuCapsule (string xmlpath)
		{
			Scene m_capsula = SceneManager.GetScene("menu_capsule");
			ObfItem item = (ObfItem) FileG.ReadXML(xmlpath, typeof(ObfItem));
			Tuple<int,int> screensz = Utis.GetScreenSize();

			Console.WriteLine(item.ToString());

			Button exec = Elements.CreateButton("Executar",new Point(10,10),new Size(120,50));
			Button edit = Elements.CreateButton("Editar dados",new Point(10,70),new Size(120,50));
			Button quit = Elements.CreateButton("Sair",new Point(10,130),new Size(120,50));
			
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

			comments.CheckState = item.comments ? CheckState.Checked : CheckState.Unchecked;

			CheckBox linebreaks = Elements.CreateCheckbox("Permite quebra de linha",new Point(screensz.Item1 - 222, 340),new Size(200,40));

			linebreaks.CheckState = item.linebreaks ? CheckState.Checked : CheckState.Unchecked;

			m_capsula.AddToScene(exec);
			m_capsula.AddToScene(edit);
			m_capsula.AddToScene(quit);
			m_capsula.AddToScene(linename);
			m_capsula.AddToScene(linedesc);
			m_capsula.AddToScene(comments);
			m_capsula.AddToScene(linebreaks);

			// Executa procedimentos do pipeline
			exec.Click += (s, e) =>
			{
				Steps.MakeOutFile(item, xmlpath);
			};

			// Salva os dados do pipeline no disco
			edit.Click += (s, e) =>
			{
				SceneManager.CreateScene("menu_edit");

				m_capsula.DisableScene();
				SceneManager.GetScene("menu_edit").EnableScene();

				CreateEditMenu(item, xmlpath);
			};

			quit.Click += (s, e) =>
			{
				SceneManager.GetScene("menu_pre").EnableScene();
				m_capsula.DestroyScene();
				FileG.ReadMainFile();
			};
		}

		public static void CreateEditMenu (ObfItem item, string path)
		{
			Scene m_edit = SceneManager.GetScene("menu_edit");
			Tuple<int,int> screensz = Utis.GetScreenSize();

			Button save = Elements.CreateButton("Gravar dados",new Point(10,10),new Size(120,50));
			Button exit = Elements.CreateButton("Voltar",new Point(10,70),new Size(120,50));

			// Nome do item
			TextBox name = Elements.CreateTextbox(item.name,
				new Point(screensz.Item1/2 - 200,screensz.Item2/2 - 80),new Size(400,40));
			// Descrição do item
			RichTextBox desc = Elements.CreateRichTextbox(item.desc,
				new Point(screensz.Item1/2 - 200, screensz.Item2/2 - 40),new Size(400,100));
			// Nome de saída do arquivo
			TextBox outname = Elements.CreateTextbox(item.outname,
				new Point(screensz.Item1/2 - 200,screensz.Item2/2 + 100),new Size(400,40));
			// Diretório de saída
			TextBox outdir = Elements.CreateTextbox(item.outdir,
				new Point(screensz.Item1/2 - 200,screensz.Item2/2 + 140),new Size(400,40));
			
			// Permite comentários
			CheckBox comments = Elements.CreateCheckbox("Permite comentários",
				new Point(screensz.Item1/2 - 100, 180),new Size(200,40));
			// Permite quebra de linha 
			CheckBox linebreaks = Elements.CreateCheckbox("Permite quebra de linha",
				new Point(screensz.Item1/2 - 100, 210),new Size(200,40));

            comments.CheckState = item.comments ? CheckState.Checked : CheckState.Unchecked;
            linebreaks.CheckState = item.linebreaks ? CheckState.Checked : CheckState.Unchecked;

			m_edit.AddToScene(save);
			m_edit.AddToScene(exit);
			m_edit.AddToScene(name);
			m_edit.AddToScene(desc);
			m_edit.AddToScene(outname);
			m_edit.AddToScene(outdir);
			m_edit.AddToScene(comments);
			m_edit.AddToScene(linebreaks);

			Action quitEdit = () =>
			{
				SceneManager.GetScene("menu_capsule").EnableScene();
				SceneManager.RemoveScene(m_edit.GetSceneName());
			};

			save.Click += (s, e) =>
			{
				ObfItem neoitem = new ObfItem()
				{
					name = name.Text,
					desc = desc.Text,
					includes = item.includes,
					header = item.header,
					outdir = outdir.Text,
					outname = outname.Text,
					comments = comments.Checked,
					linebreaks = linebreaks.Checked,
					obfuscate = item.obfuscate,
				};

				FileG.WriteXML(path, neoitem);

				quitEdit();
			};

			exit.Click += (s, e) =>
			{
				quitEdit();
			};
		}
	}
}
