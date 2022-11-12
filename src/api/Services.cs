using api.Models;

namespace api
{
	public class Services
	{
		public static List<int> GetTopValues(List<Insurance> insurances, int maxCount, int maxDepth)
		{
			List<int> topValues = new List<int>(maxCount);

			foreach (Insurance insurance in insurances)
			{
				topValues.Add(insurance.CombinedValue(maxDepth));
			}

			//Placing them in descending order
			topValues.Sort();
			topValues.Reverse();

			//Removing everything except the top X insurances
			for (int i = topValues.Count - 1; i >= maxCount; i--)
			{
				topValues.RemoveAt(i);
			}

			return topValues;
		}
	}
}
