using System;

namespace General.Utilities.Text.Similarity
{
	/// <summary>
	/// Summary description for ISimilarity.
	/// </summary>
	interface ISimilarity
	{
		float GetSimilarity(string string1, string string2);
	}
}
