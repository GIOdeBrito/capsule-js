namespace capsulajs
{
	public partial class Form1 : Form
	{
		private static Form1 _form;

		public Form1()
		{
			InitializeComponent();
			FireTimer();

			_form = this;
		}

		public static Form1 GetForm()
		{
			return _form;
		}

		private void FireTimer()
		{
			System.Windows.Forms.Timer tmr = new System.Windows.Forms.Timer();
			// Milisegundos
			tmr.Interval = 16;
			// A cada 'tick' esta função será executada
			tmr.Tick += Tmr_Tick;
			tmr.Stop();
			tmr.Start();
		}

		private void Tmr_Tick(object sender, EventArgs e)
		{
			Snow.TranslateSnow();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Start.BeginAgain();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			Snow.DestroyAll();
			SceneManager.DestroyAllScenes();
		}
	}
}