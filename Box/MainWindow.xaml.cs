using System.Windows;
using Sand;
using Point = Sand.Point;

namespace Box
{
	public partial class MainWindow : Window
	{
		private const int Size = 100;
		private readonly SandColumn[,] m_sand = SandTable.CreateSandTable(Size, Size, 15);
		private readonly Point m_holeLocation;
		private readonly int?[,] m_spherePattern = HeightLimitPatternLibrary.Sphere(20);

		public MainWindow()
		{
			InitializeComponent();
			m_holeLocation = new Point {X = 15, Y = 15, Height = 15};
		}

		private void NextButton_OnClick(object sender, RoutedEventArgs e)
		{
			//((MainViewModel)this.DataContext).AddBox(new Sand.Point{X=0,Y=0}, 1,1,1);
			((MainViewModel)DataContext).Clear();
			m_holeLocation.X++;
			m_holeLocation.Y++;

			SandTable.ClearHeightLimits(m_sand);
			SandTable.ApplyHeightLimitPattern(m_sand, m_spherePattern, m_holeLocation);
			SandTable.SettleMapTwoPass(m_sand);

			((MainViewModel)DataContext).StartAdding();
			foreach (var column in m_sand)
			{
				((MainViewModel)DataContext).AddBox(column.Location, 1, 1, column.Height);

			}
			((MainViewModel)DataContext).FinishAdding();
		}
	}
}
