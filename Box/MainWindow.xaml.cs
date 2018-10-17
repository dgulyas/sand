using System.Windows;
using Sand;
using Point = Sand.Point;

namespace Box
{
	public partial class MainWindow : Window
	{
		private const int Size = 23;
		private readonly SandColumn[,] m_sand = SandTable.CreateSandTable(Size, Size, 15);

		public MainWindow()
		{
			InitializeComponent();
		}

		private void NextButton_OnClick(object sender, RoutedEventArgs e)
		{
			//((MainViewModel)this.DataContext).AddBox(new Sand.Point{X=0,Y=0}, 1,1,1);
			((MainViewModel)DataContext).Clear();


			SandTable.ClearHeightLimits(m_sand);
			SandTable.ApplyHeightLimitPattern(m_sand, HeigthLimitPatternLibrary.SmallCube, new Point { X = 10, Y = 10, Height = 10 });
			SandTable.SettleMapTwoPass(m_sand);

			foreach (var column in m_sand)
			{
				((MainViewModel)DataContext).AddBox(column.Location, 1, 1, column.Height);
			}
		}
	}
}
