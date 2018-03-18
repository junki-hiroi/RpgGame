using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleCharacter
{
	public int Id;
	public string Name;
	public bool IsEnemy;
	public int HitPoint;
	public int MaxHitPoint;
	public int Attack;
	public int Speed;
	public int Current;

	public BattleCharacter(int id, string name, bool isEnemy, int maxHitPoint, int attack, int speed)
	{
		Id = id;
		Name = name;
		IsEnemy = isEnemy;
		MaxHitPoint = maxHitPoint;
		Attack = attack;
		Speed = speed;

		HitPoint = maxHitPoint;
		Current = 0;
	}
}

public class TargetInfo
{
	public enum TargetModeEnum
	{
		SelectId,
		SelectTeamMy,
		SelectTeamEnemy,
	}

	public TargetModeEnum TargetMode;
	public int Id;

	public static TargetInfo CreateTargetId(int id)
	{
		return new TargetInfo
		{
			Id = id,
			TargetMode = TargetModeEnum.SelectId
		};
	}
}

public class BattleCommand
{
	public int ActorId;
	public BattleActionLibrary.BattleAction ActionCallback;
	public TargetInfo Target;
}

public class BattleManager
{
	private static BattleManager _instance = null;
	public static BattleManager Instance { get { return _instance; }}

	private System.Object managerLock = new System.Object();  

	public Dictionary<int, BattleCharacter> Characters = new Dictionary<int, BattleCharacter>();
	
	public static BattleManager CreateBattleManager(BattleCharacter my, BattleCharacter enemy)
	{
		_instance = new BattleManager();
		_instance.Characters.Add(my.Id, my);
		_instance.Characters.Add(enemy.Id, enemy);
		return _instance;
	}
	
	public void Wait()
	{
		lock (managerLock)
		{
			foreach (var character in Characters.Values)
			{
				character.Current = character.Current + character.Speed;
			}
		}
	}
	
	public void Command(BattleCommand command)
	{
		lock (managerLock)
		{
			List<int> targets = GetTargetIds(command.Target);

			foreach (var target in targets)
			{
				command.ActionCallback(Characters[command.ActorId], Characters[target]);
			
				Characters[target].HitPoint = Math.Min(Characters[target].MaxHitPoint, Math.Max(0, Characters[target].HitPoint));

				if (Characters[target].HitPoint == 0)
				{
					Debug.Log(Characters[target].Name + " は たおれた.");
				}
			}
		
			Characters[command.ActorId].Current = 0;
		}
	}

	private List<int> GetTargetIds(TargetInfo targetInfo)
	{
		if (targetInfo.TargetMode == TargetInfo.TargetModeEnum.SelectId)
		{
			return new List<int>(){targetInfo.Id};
		}
		else if (targetInfo.TargetMode == TargetInfo.TargetModeEnum.SelectTeamMy)
		{
			return Characters.Where((pair => !pair.Value.IsEnemy)).Select((pair => pair.Key)).ToList();
		}
		else
		{
			return Characters.Where((pair => pair.Value.IsEnemy)).Select((pair => pair.Key)).ToList();
		}
	}

	public bool IsWin()
	{
		List<int> targets = GetTargetIds(new TargetInfo{ TargetMode = TargetInfo.TargetModeEnum.SelectTeamEnemy});
		return Characters.Where((pair => targets.Contains(pair.Key))).All((pair => pair.Value.HitPoint == 0));
	}
	
	public bool IsLose()
	{
		List<int> targets = GetTargetIds(new TargetInfo{ TargetMode = TargetInfo.TargetModeEnum.SelectTeamMy});
		return Characters.Where((pair => targets.Contains(pair.Key))).All((pair => pair.Value.HitPoint == 0));
	}
}
