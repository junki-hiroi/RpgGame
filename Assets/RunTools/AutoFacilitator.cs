#if UN_AUTO_DEBUG
using System.Collections;
using UnityEngine;

namespace RunTools
{
	public class AutoFacilitator : MonoBehaviour
	{
		private IAuto _auto = null;

		public void SetWatch(IAuto auto)
		{
			_auto = auto;
			StartCoroutine(PlayerWatch());
		}

		private IEnumerator PlayerWatch()
		{
			while(true)
			{
				var commands = _auto.CreateCommands();
				if (commands.Count != 0)
				{
					var commandNo = new System.Random().Next(commands.Count);
					// Debug.Log(commands[commandNo].Visual);
					commands[commandNo].Exec();
				}
			
				yield return new WaitForSeconds(0.3f);
			}
		}
	}
}
#endif
