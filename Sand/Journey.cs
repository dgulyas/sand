using System;
using System.Collections.Generic;

namespace Sand
{
	public class Journey
	{
		public Queue<Path> Paths = new Queue<Path>();

		private Path m_currentPath;
		private Point m_currentLocation;

		private Point m_lastPoint;
		private Point m_nextPoint;
		private double m_totalSteps;
		private double m_currentStep;

		public Journey(Point startingOffset)
		{
			m_currentLocation = startingOffset;
		}

		public void AddPath(Path path)
		{
			if (!path.Empty())
			{
				Paths.Enqueue(path);
			}
		}

		public Point GetNextLocation()
		{
			if (m_currentPath == null || m_currentPath.Empty())
			{
				if (Paths.Count > 0)
				{
					m_currentPath = Paths.Dequeue();
					m_currentPath.Offset = m_currentLocation;
				}
				else
				{
					return null;
				}
			}

			m_currentLocation = m_currentPath.GetNextPoint();
			return m_currentLocation;
		}


		public Point TweenLocations()
		{
			if (m_lastPoint == null)
			{
				m_lastPoint = GetNextLocation();
			}

			if (m_nextPoint == null)
			{
				m_nextPoint = GetNextLocation();
				if (m_nextPoint == null)
				{
					return null;
				}
				m_totalSteps = Math.Max(Math.Max(Math.Abs(m_nextPoint.X - m_lastPoint.X), Math.Abs(m_nextPoint.Y - m_lastPoint.Y)), Math.Abs(m_nextPoint.Height - m_lastPoint.Height));
				m_currentStep = 0;
			}

			var xMove = m_nextPoint.X - m_lastPoint.X;
			var yMove = m_nextPoint.Y - m_lastPoint.Y;
			var heightMove = m_nextPoint.Height - m_lastPoint.Height;

			var factor = m_currentStep / m_totalSteps;

			var tweenPoint = new Point
			{
				X = (int)(xMove * factor) + m_lastPoint.X,
				Y = (int)(yMove * factor) + m_lastPoint.Y,
				Height = (int)(heightMove * factor) + m_lastPoint.Height,
			};

			m_currentStep++;

			if (m_currentStep >= m_totalSteps)
			{
				m_lastPoint = m_nextPoint;
				m_nextPoint = null;
			}

			return tweenPoint;

		}

	}
}
