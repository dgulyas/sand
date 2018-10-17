using System.IO;
using System.Windows;
using Sand;

namespace Box
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const int Size = 23;
		private SandColumn[,] sand = SandTable.CreateSandTable(Size, Size, 10);

		public MainWindow()
		{
			InitializeComponent();
		}

		private void NextButton_OnClick(object sender, RoutedEventArgs e)
		{
			File.WriteAllText(Path.Combine(@"C:\Users\dgulyas\Desktop\out", "buttonClick"), "Thing");
		}
	}
}
