using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

using Sand;

using XInput.Wrapper;

using Point = Sand.Point;

namespace Box
{
	public partial class MainWindow : Window
	{
		private const int Size = 120;
		private const int SandLevel = 15;
		private SandColumn[,] m_sand = SandTable.CreateSandTable(Size, Size, SandLevel);
		private readonly Point m_holeLocation;
		private const int Diameter = 10;
		private const double Radius = Diameter / 2.0;
		private readonly int?[,] m_spherePattern = HeightLimitPatternLibrary.Sphere(Diameter);
		private IAnimator animator;
		private readonly X.Gamepad gamepad;
		private bool closed;

		private Dictionary<X.Gamepad.GamepadButtons, bool> pressedButtons = new Dictionary<X.Gamepad.GamepadButtons, bool>();

		private Barrier drawSync = new Barrier(2);

		public MainWindow()
		{
			InitializeComponent();
			m_holeLocation = new Point { X = Size / 2, Y = Size / 2, Height = 30 };
			((MainViewModel)DataContext).AddMeshHeights(m_sand);
			((MainViewModel)DataContext).AddSphere(new Point3D(m_holeLocation.X + Radius, m_holeLocation.Y + Radius, m_holeLocation.Height), Radius);
			((MainViewModel)DataContext).AddContainer(Size, Size, SandLevel);

			HelixPane.Camera.Position = new Point3D(119.1, -187.8, 353.7);
			HelixPane.Camera.LookDirection = new Vector3D(3.4, 310.3, -346.2);

			if (X.IsAvailable)
			{
				gamepad = X.Gamepad_1;
				gamepad.StateChanged += ProcessXboxInput;
				X.StartPolling(gamepad);
			}

			Closing += (sender, args) =>
			{
				X.StopPolling();
				drawSync.RemoveParticipant();
				closed = true;
			};
		}

		private void ProcessXboxInput(object sender, EventArgs e)
		{
			ProcessXboxKeyPress(sender, e);
			ProcessXboxDpadPress(sender, e);

			foreach (var button in Enum.GetValues(typeof(X.Gamepad.GamepadButtons)))
			{
				pressedButtons[(X.Gamepad.GamepadButtons)button] = (gamepad.Buttons & (short)(int)button) != 0;
			}
		}

		private bool IsPressed(X.Gamepad.GamepadButtons button)
		{
			if (!pressedButtons.ContainsKey(button))
			{
				return false;
			}

			var value = !pressedButtons[button] && (gamepad.Buttons & (short)(int)button) != 0;
			return value;
		}

		private void ProcessXboxKeyPress(object sender, EventArgs e)
		{
			if (IsPressed(X.Gamepad.GamepadButtons.LBumper))
			{
				this.Dispatcher.BeginInvoke(new Action(() =>
				{
					StatusTextBox.Text = "Up";
					Move(new Point { Height = 1 });
				}));
			}
			else if (IsPressed(X.Gamepad.GamepadButtons.RBumper))
			{
				this.Dispatcher.BeginInvoke(new Action(() =>
				{
					StatusTextBox.Text = "Down";
					Move(new Point { Height = -1 });
				}));
			}
			else if (IsPressed(X.Gamepad.GamepadButtons.X) || IsPressed(X.Gamepad.GamepadButtons.LeftStick))
			{
				this.Dispatcher.BeginInvoke(new Action(() =>
				{
					animator = new DropAnimator(Diameter);
					ApplyBallAndDisplaySand();
				}));
			}

		}

		private void ProcessXboxDpadPress(object sender, EventArgs e)
		{
			Point yolo = null;
			if (IsPressed(X.Gamepad.GamepadButtons.Dpad_Up))
			{
				yolo = new Point { Y = 1 };
			}
			else if (IsPressed(X.Gamepad.GamepadButtons.Dpad_Down))
			{
				yolo = new Point { Y = -1 };
			}
			else if (IsPressed(X.Gamepad.GamepadButtons.Dpad_Left))
			{
				yolo = new Point { X = -1 };
			}
			else if (IsPressed(X.Gamepad.GamepadButtons.Dpad_Right))
			{
				yolo = new Point { X = 1 };
			}

			if (yolo != null)
			{
				this.Dispatcher.BeginInvoke(new Action(() =>
				{
					Move(yolo);
				}));
			}
		}

		private void DoWork()
		{
			this.Dispatcher.BeginInvoke(new Action(() =>
			{
				foreach (var control in this.ButtonGrid.Children)
				{
					if (control is Button)
					{
						(control as Button).IsEnabled = false;
					}
				}
				this.Stop.IsEnabled = true;
			}));

			while (!closed)
			{
				// update
				var nextMove = animator?.GetNext();
				animator?.CheckPosition(m_holeLocation, m_sand);

				if (nextMove != null)
				{
					m_holeLocation.X += nextMove.Value.X;
					m_holeLocation.Y += nextMove.Value.Y;
					m_holeLocation.Height += nextMove.Value.Depth;
				}

				// physics
				this.Dispatcher.BeginInvoke(new Action(() =>
				{
					StatusTextBox.Text = "Doing Physics";
				}));


				SandTable.ClearHeightLimits(m_sand);
				SandTable.ApplyHeightLimitPattern(m_sand, m_spherePattern, m_holeLocation);
				SandTable.SettleMapTwoPassMinimally(m_sand);

				// render
				this.Dispatcher.BeginInvoke(new Action(() =>
				{
					((MainViewModel)DataContext).Clear();
					StatusTextBox.Text = "Rendering";
					((MainViewModel)DataContext).AddMeshHeights(m_sand);
					((MainViewModel)DataContext).AddSphere(new Point3D(m_holeLocation.X + Radius, m_holeLocation.Y + Radius, m_holeLocation.Height), Radius);
					((MainViewModel)DataContext).AddContainer(Size, Size, SandLevel);

					StatusTextBox.Text = "Done";
					drawSync.SignalAndWait();
				}));
				drawSync.SignalAndWait();

				if (nextMove == null || animator == null)
				{
					break;
				}
			}

			animator = null;

			this.Dispatcher.BeginInvoke(new Action(() =>
			{
				foreach (var control in this.ButtonGrid.Children)
				{
					if (control is Button)
					{
						(control as Button).IsEnabled = true;
					}
				}
			}));
		}

		private void ApplyBallAndDisplaySand()
		{
			var thread = new Thread(() => DoWork());
			thread.Start();
		}

		private bool Move(Point movement)
		{
			var newCenter = m_holeLocation + movement;
			newCenter.X += Diameter / 2;
			newCenter.Y += Diameter / 2;
			newCenter.Height -= (Diameter / 2) + 2;

			if (newCenter.X < 0 || newCenter.X >= Size || newCenter.Y < 0 || newCenter.Y >= Size || newCenter.Height < 0 || newCenter.Height > 100)
			{
				return false;
			}
			else
			{
				var newPosition = m_holeLocation + movement;
				m_holeLocation.X = newPosition.X;
				m_holeLocation.Y = newPosition.Y;
				m_holeLocation.Height = newPosition.Height;
				ApplyBallAndDisplaySand();
				return true;
			}


		}

		private void MoveUpLeft_OnClick(object sender, RoutedEventArgs e)
		{
			Move(new Point { X = -1, Y = 1 });
		}

		private void MoveUp_OnClick(object sender, RoutedEventArgs e)
		{
			Move(new Point { Y = 1 });
		}

		private void MoveUpRight_OnClick(object sender, RoutedEventArgs e)
		{
			Move(new Point { X = 1, Y = 1 });
		}

		private void MoveLeft_OnClick(object sender, RoutedEventArgs e)
		{
			Move(new Point { X = -1 });
		}

		private void MoveRight_OnClick(object sender, RoutedEventArgs e)
		{
			Move(new Point { X = 1 });
		}

		private void MoveDownLeft_OnClick(object sender, RoutedEventArgs e)
		{
			Move(new Point { X = -1, Y = -1 });
		}

		private void MoveDown_OnClick(object sender, RoutedEventArgs e)
		{
			Move(new Point { Y = -1 });
		}

		private void MoveDownRight_OnClick(object sender, RoutedEventArgs e)
		{
			Move(new Point { X = 1, Y = -1 });
		}

		private void MoveZUp_OnClick(object sender, RoutedEventArgs e)
		{
			Move(new Point { Height = 1 });
		}

		private void MoveZDown_OnClick(object sender, RoutedEventArgs e)
		{
			Move(new Point { Height = -1 });
		}
		private void Drop_Click(object sender, RoutedEventArgs e)
		{
			animator = new DropAnimator(Diameter);
			ApplyBallAndDisplaySand();
		}

		private void Animate_Click(object sender, RoutedEventArgs e)
		{
			animator = JourneyAnimator.FromVecList(m_trianglePath, Size, Size, m_holeLocation.Height, m_holeLocation);
			ApplyBallAndDisplaySand();
		}
		private void Stop_Click(object sender, RoutedEventArgs e)
		{
			animator = null;
		}
		private void Clear_Click(object sender, RoutedEventArgs e)
		{
			m_sand = SandTable.CreateSandTable(Size, Size, SandLevel);
			ApplyBallAndDisplaySand();
		}

		private const string m_testPath = "{vec2(-0.8203125,0.80078125),vec2(-0.73046875,0.82421875),vec2(-0.6328125,0.82421875),vec2(-0.4765625,0.82421875),vec2(-0.27734375,0.8125),vec2(-0.07421875,0.78125),vec2(0.125,0.74609375),vec2(0.2109375,0.65234375),vec2(0.3046875,0.5703125),vec2(0.39453125,0.453125),vec2(0.47265625,0.359375),vec2(0.5234375,0.28515625),vec2(0.5859375,0.16015625),vec2(0.66015625,-0.015625),vec2(0.70703125,-0.13671875),vec2(0.7421875,-0.28515625),vec2(0.76953125,-0.46484375),vec2(0.7578125,-0.5859375),vec2(0.7109375,-0.671875),vec2(0.6171875,-0.7734375),vec2(0.4296875,-0.875),vec2(0.30859375,-0.88671875),vec2(0.1328125,-0.87890625),vec2(-0.015625,-0.875),vec2(-0.1640625,-0.89453125),vec2(-0.3203125,-0.89453125),vec2(-0.4921875,-0.89453125),vec2(-0.59375,-0.89453125),vec2(-0.76171875,-0.77734375),vec2(-0.7890625,-0.60546875),vec2(-0.7890625,-0.45703125),vec2(-0.796875,-0.30859375),vec2(-0.8046875,-0.11328125),vec2(-0.81640625,0.06640625),vec2(-0.81640625,0.26171875),vec2(-0.80078125,0.48046875),vec2(-0.73828125,0.5859375),vec2(-0.59765625,0.5859375),vec2(-0.37890625,0.56640625),vec2(-0.2578125,0.55859375),vec2(-0.09765625,0.47265625),vec2(0.0546875,0.3828125),vec2(0.16015625,0.25),vec2(0.25390625,0.11328125),vec2(0.3203125,-0.015625),vec2(0.33984375,-0.109375),vec2(0.3515625,-0.24609375),vec2(0.3515625,-0.33984375),vec2(0.3359375,-0.42578125),vec2(0.3046875,-0.52734375),vec2(0.2421875,-0.54296875),vec2(0.0546875,-0.546875),vec2(-0.0703125,-0.5546875),vec2(-0.21875,-0.5546875),vec2(-0.34765625,-0.55859375),vec2(-0.48046875,-0.57421875),vec2(-0.52734375,-0.44921875),vec2(-0.546875,-0.28125),vec2(-0.5546875,-0.05859375),vec2(-0.54296875,0.10546875),vec2(-0.4375,0.234375),vec2(-0.296875,0.25390625),vec2(-0.08203125,0.2109375),vec2(0.078125,0.109375),vec2(0.15234375,-0.04296875),vec2(0.171875,-0.15625),vec2(0.171875,-0.296875),vec2(0.0234375,-0.359375),vec2(-0.09375,-0.37109375),vec2(-0.234375,-0.37109375),vec2(-0.3125,-0.29296875),vec2(-0.33984375,-0.13671875),vec2(-0.3046875,-0.046875),vec2(-0.23046875,-0.02734375),vec2(-0.1171875,-0.0625),vec2(-0.0546875,-0.14453125),vec2(-0.06640625,-0.19140625),vec2(-0.1328125,-0.2265625),vec2(-0.1640625,-0.2265625)}";
		private const string m_invidiPath = "{vec2(-0.875,0.80078125),vec2(0.91015625,0.80078125),vec2(0.00390625,0.8125),vec2(0,-0.80078125),vec2(-0.8984375,-0.796875),vec2(0.890625,-0.80859375),vec2(-0.88671875,-0.828125),vec2(-0.8515625,0.77734375),vec2(0.8515625,-0.796875),vec2(0.90234375,0.78515625),vec2(0.0234375,-0.77734375),vec2(-0.83984375,0.75),vec2(0.8671875,0.78515625),vec2(0.0078125,0.76953125),vec2(-0.03515625,-0.7734375),vec2(-0.94140625,-0.796875),vec2(0.9375,-0.79296875),vec2(-0.91796875,-0.82421875),vec2(0.0859375,-0.7578125),vec2(0.5234375,-0.45703125),vec2(0.6640625,0.00390625),vec2(0.53125,0.57421875),vec2(0.1015625,0.77734375),vec2(-0.8828125,0.72265625),vec2(-0.9453125,-0.76171875),vec2(0.90625,-0.76171875),vec2(-0.01953125,-0.828125),vec2(-0.02734375,0.81640625),vec2(0.9453125,0.81640625),vec2(-0.90234375,0.828125)}";
		private const string m_invidiWordPath = "{vec2(-0.8984375,0.203125),vec2(-0.6953125,0.203125),vec2(-0.8046875,0.20703125),vec2(-0.8046875,0.00390625),vec2(-0.90234375,0.01953125),vec2(-0.7109375,0.01171875),vec2(-0.6015625,0),vec2(-0.59375,0.1953125),vec2(-0.41015625,0.00390625),vec2(-0.4140625,0.19140625),vec2(-0.41015625,0.00390625),vec2(-0.203125,-0.00390625),vec2(-0.296875,0.19140625),vec2(-0.203125,0.00390625),vec2(-0.09765625,0.1953125),vec2(-0.203125,-0.00390625),vec2(0.00390625,0.0078125),vec2(0.1953125,-0.00390625),vec2(0.09375,-0.00390625),vec2(0.09375,0.1953125),vec2(-0.00390625,0.1953125),vec2(0.1953125,0.203125),vec2(0.09375,0.19140625),vec2(0.08984375,0.00390625),vec2(0.296875,0.00390625),vec2(0.30078125,0.203125),vec2(0.39453125,0.16796875),vec2(0.4296875,0.09765625),vec2(0.3984375,0.046875),vec2(0.31640625,0),vec2(0.5,0),vec2(0.69921875,0.0078125),vec2(0.6015625,0.0078125),vec2(0.59765625,0.203125),vec2(0.703125,0.1953125),vec2(0.4921875,0.1953125)}";
		private const string m_trianglePath = "{vec2(-0.8984375,-0.8984375),vec2(0,0.8984375),vec2(0.796875,-0.890625),vec2(-0.89453125,-0.69140625),vec2(0.203125,0.8984375),vec2(0.59375,-0.89453125),vec2(-0.90625,-0.50390625),vec2(0.3984375,0.89453125),vec2(0.38671875,-0.890625),vec2(-0.90234375,-0.30078125),vec2(0.6015625,0.8984375),vec2(0.19921875,-0.90234375),vec2(-0.89453125,-0.1015625),vec2(0.7890625,0.90625),vec2(0,-0.89453125),vec2(-0.8984375,0.1015625),vec2(0.79296875,0.69921875),vec2(-0.203125,-0.88671875),vec2(-0.8984375,0.296875),vec2(0.796875,0.48828125),vec2(-0.3984375,-0.90234375),vec2(-0.90234375,0.50390625),vec2(0.8046875,0.29296875),vec2(-0.60546875,-0.90234375),vec2(-0.8984375,0.69140625),vec2(0.80078125,0.1015625),vec2(-0.80078125,-0.89453125),vec2(-0.90234375,0.90234375),vec2(0.8046875,-0.1015625),vec2(-0.8984375,-0.80078125),vec2(-0.703125,0.89453125),vec2(0.796875,-0.31640625),vec2(-0.91015625,-0.6015625),vec2(-0.50390625,0.90234375),vec2(0.79296875,-0.5),vec2(-0.90625,-0.40234375),vec2(-0.3125,0.91015625),vec2(0.79296875,-0.69921875),vec2(-0.89453125,-0.203125),vec2(-0.1328125,0.89453125),vec2(0.79296875,-0.88671875)}";
	}
}
