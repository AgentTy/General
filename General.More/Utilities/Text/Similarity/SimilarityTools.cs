using System;
using General;

namespace General.Utilities.Text.Similarity
{
	/// <summary>
	/// SimilarityTools
	/// </summary>
	public class SimilarityTools
	{

		#region Constructors

		/// <summary>
		/// SimilarityTools
		/// </summary>
		public SimilarityTools()
		{

		}

		#endregion

		#region Public Methods
		
		/// <summary>
		/// Returns a float between 0 and 1 to represent the similarity between two strings
		/// </summary>
		public static float GetSimilarity(string String1, string String2)
		{
			MatchsMaker m = new MatchsMaker(String1, String2);
			return m.Score;
		}
		#endregion

	}
}
