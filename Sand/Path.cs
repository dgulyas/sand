using System.Collections.Generic;
using System.Linq;

namespace Sand
{
	public class Path
	{
		private readonly Queue<Point> m_points;
		private readonly bool m_repeat;
		public Point Offset;

		public Path(Queue<Point> points, bool repeat = false)
		{
			m_points = points;
			m_repeat = repeat;
		}

		public Point GetNextPoint()
		{
			if (m_points.Count > 0)
			{
				var next = m_points.Dequeue();
				if (m_repeat)
				{
					m_points.Enqueue(next);
				}
				return next + Offset;
			}

			return null;
		}

		public bool Empty()
		{
			return m_points.Count == 0;
		}

		public void SetStartingPoint(Point startingPoint)
		{
			Offset = startingPoint;
		}

		public static Path FromVecList(string vecString, int depth, int maxX, int maxY, bool repeat = false)
		{
			var queue = new Queue<Point>();
			var regex = new System.Text.RegularExpressions.Regex(@"vec2\((.*?),(.+?)\)?}?$");
			var points = vecString.Split(new[] { ")," }, System.StringSplitOptions.RemoveEmptyEntries)
				.Select(vector =>
				{
					var match = regex.Match(vector);
					var x = ((double.Parse(match.Groups[1].Value) + 1) / 2.0) * (maxX - 1);
					var y = ((double.Parse(match.Groups[2].Value) + 1) / 2.0) * (maxY - 1);
					return new Point() { X = (int)x, Y = (int)y, Height = depth };
				});

			foreach (var point in points)
			{
				queue.Enqueue(point);
			}

			return new Path(queue, repeat);
		}

	}
}
