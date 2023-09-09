using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capsulajs
{
	internal class SceneManager
	{
		private static Scene[] _scenes;

		public static Scene GetScene (string name)
		{
			foreach(Scene scene in _scenes)
			{
				if(scene.GetSceneName() != name)
				{
					continue;
				}

				return scene;
			}

			return null;
		}
		
		public static void CreateScene (string name)
		{
			List<Scene> scenelist = new List<Scene>();

			if(_scenes != null && _scenes.Length > 0)
			{
				scenelist = _scenes.ToList();
			}

			Scene neo = new Scene(name);
			scenelist.Add(neo);

			_scenes = scenelist.ToArray();
		}

		public static void RemoveScene (string name)
		{
			if(_scenes.Length == 0)
			{
				return;
			}
			
			foreach(Scene scene in _scenes)
			{
				if(scene.GetSceneName() != name)
				{
					continue;
				}

				scene.DestroyScene();
					
				List<Scene> scenelist = _scenes.ToList();
				scenelist.Remove(scene);
				_scenes = scenelist.ToArray();
			}
		}

		public static void DestroyAllScenes ()
		{
			foreach(Scene scene in _scenes)
			{
				RemoveScene(scene.GetSceneName());
			}
		}
	}

	class Scene
	{
		private string _scenename = string.Empty;
		private Control[] _controls;

		public Scene (string name)
		{
			_scenename = name;
		}

		public string GetSceneName ()
		{
			return _scenename;
		}

		public void AddToScene (Control elem)
		{
			List<Control> scenecontrols = new List<Control>();

			if(_controls != null && _controls.Length > 0)
			{
				scenecontrols = _controls.ToList();
			}

			scenecontrols.Add(elem);
			_controls = scenecontrols.ToArray();

			Form1.GetForm().Controls.Add(elem);
			elem.BringToFront();
		}

		public bool ControlIsEmpty ()
		{
			if(_controls is null || _controls.Length <= 0)
			{
				return true;
			}

			return false;
		}

		public void EnableScene ()
		{
			if(ControlIsEmpty())
			{
				return;
			}
			
			foreach(Control elem in _controls)
			{
				if(elem is null)
				{
					return;
				}
				
				elem.Visible = true;
			}
		}

		public void DisableScene ()
		{
			if(ControlIsEmpty())
			{
				return;
			}
			
			foreach(Control elem in _controls)
			{
				elem.Visible = false;
			}
		}

		public void DestroyScene ()
		{
			if(ControlIsEmpty())
			{
				return;
			}
			
			foreach(Control elem in _controls)
			{
				Elements.DisposeElement(elem);
				Console.WriteLine(elem.Name + " disposed.");
			}
		}
	}
}
