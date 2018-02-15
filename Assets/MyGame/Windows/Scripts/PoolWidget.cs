using System.Collections;
using System.Collections.Generic;
using Core.Services.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CoreDemo
{
	/// <summary>
	/// Widget displays the Pool information.
	/// </summary>
	public class PoolWidget : UIWidget
	{
		[SerializeField]
		Text poolSizeLimitText;

		[SerializeField]
		Text activePoolElementsText;

		/// <summary>
		/// Updates the Text values
		/// </summary>
		/// <param name="size"></param>
		/// <param name="active"></param>
		public void UpdateWidgetValue(int size, int active)
		{
			poolSizeLimitText.text = size.ToString();
			activePoolElementsText.text = active.ToString();
		}
	}
}