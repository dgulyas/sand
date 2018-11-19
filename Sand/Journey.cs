using System;
using System.Collections.Generic;

namespace Sand
{
	public class Journey
	{
		public Queue<Path> Paths = new Queue<Path>();

		private Path m_currentPath;
		private Point m_currentLocation;

		private Point lastPoint;
		private Point nextPoint;
		private double totalSteps;
		private double currentStep;

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
			if (lastPoint == null)
			{
				lastPoint = this.GetNextLocation();
			}

			if (nextPoint == null)
			{
				nextPoint = this.GetNextLocation();
				if (nextPoint == null)
				{
					return null;
				}
				totalSteps = Math.Max(Math.Max(Math.Abs(nextPoint.X - lastPoint.X), Math.Abs(nextPoint.Y - lastPoint.Y)), Math.Abs(nextPoint.Height - lastPoint.Height));
				currentStep = 0;
			}

			var xMove = nextPoint.X - lastPoint.X;
			var yMove = nextPoint.Y - lastPoint.Y;
			var heightMove = nextPoint.Height - lastPoint.Height;

			var factor = currentStep / totalSteps;

			var tweenPoint = new Point
			{
				X = (int)(xMove * factor) + lastPoint.X,
				Y = (int)(yMove * factor) + lastPoint.Y,
				Height = (int)(heightMove * factor) + lastPoint.Height,
			};

			currentStep++;

			if (currentStep >= totalSteps)
			{
				lastPoint = nextPoint;
				nextPoint = null;
			}

			return tweenPoint;

		}

	}
}
