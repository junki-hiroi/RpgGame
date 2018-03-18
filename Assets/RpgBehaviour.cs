using System.Collections.Generic;
using RunTools;
using UnityEngine;

public class RpgBehaviour :
	MonoBehaviour
#if UN_AUTO_DEBUG
	, IAuto
#endif
{
	protected void SetRpg()
	{
#if UN_AUTO_DEBUG
		if (AutoManager.IsAuto)
		{
			Time.timeScale = AutoManager.TimeScaleSetting;
			AutoFacilitator autoFacilitator = gameObject.AddComponent<AutoFacilitator>();
			autoFacilitator.SetWatch(this);
		}
#endif
	}

	public virtual List<GameCommand> CreateCommands()
	{
#if UN_AUTO_DEBUG
		throw new System.NotImplementedException();
#elif
		return new List<GameCommand>();
#endif
	}
}
