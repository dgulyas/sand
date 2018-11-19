
using Sand;

namespace Box
{
	struct Movement
	{
		public int X;
		public int Y;
		public int Depth;
	}
	interface IAnimator
	{

		Movement? GetNext();
		void CheckPosition(Point currentPosition, SandColumn[,] currentState);
	}
	class DropAnimator : IAnimator
	{
		private bool m_dropping;
		private int? m_initialTouchPoint;
		private readonly int m_radius;

		public DropAnimator(int diameter)
		{
			m_radius = diameter / 2;
			m_dropping = true;
		}

		public void CheckPosition(Point currentPosition, SandColumn[,] currentState)
		{
			if (m_initialTouchPoint == null)
			{
				m_initialTouchPoint = currentState[currentPosition.X + m_radius, currentPosition.Y + m_radius].Height;
			}

			var sandTouchPoint = currentState[currentPosition.X + m_radius, currentPosition.Y + m_radius].Height;

			m_dropping &= sandTouchPoint == m_initialTouchPoint;

		}

		public Movement? GetNext()
		{
			if (m_dropping)
			{
				return new Movement
				{
					X = 0,
					Y = 0,
					Depth = -1,
				};
			}
			else
			{
				return null;
			}
		}
	}

	class JourneyAnimator : IAnimator
	{
		private readonly Journey m_journey;
		private Point m_currentPosition;
		public JourneyAnimator(Journey journey)
		{
			m_journey = journey;
			m_currentPosition = m_journey.Paths.Peek().Offset;
		}

		public static JourneyAnimator FromVecList(string vecList, int maxX, int maxY, int depth, Point currentPosition)
		{
			var path = Path.FromVecList(vecList, depth, maxX, maxY, true);
			path.Offset = new Point { X = -currentPosition.X, Y = -currentPosition.Y, Height = -currentPosition.Height };
			var jor = new Journey(new Point { X = 0, Y = 0, Height = 0 });
			jor.AddPath(path);

			return new JourneyAnimator(jor);

		}

		public void CheckPosition(Point currentPosition, SandColumn[,] currentState)
		{
			m_currentPosition = currentPosition;
		}

		public Movement? GetNext()
		{
			var point = m_journey.TweenLocations();

			if (point == null)
			{
				return null;
			}

			var movement = new Movement
			{
				X = point.X - m_currentPosition.X,
				Y = point.Y - m_currentPosition.Y,
				Depth = point.Height - m_currentPosition.Height,
			};

			return movement;
		}
	}
}
