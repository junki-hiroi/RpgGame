using System;
using System.Collections;
using System.Collections.Generic;
using RunTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleFacilitator : RpgBehaviour {
	private BattleCharacter _my;
	private BattleCharacter _enemy;
	private Vector2 _scroll = Vector2.zero;
	private bool _battleEnd = false;

	private const float WatchTime = 0.5f;

	private void Start ()
	{
		_my = new BattleCharacter(
			id: 0,
			name: "勇者",
			isEnemy: false,
			maxHitPoint: 120,
			attack: 20,
			speed: 15
		);
		
		_enemy = new BattleCharacter(
			id: 1,
			name: "魔王",
			isEnemy: true,
			maxHitPoint: 200,
			attack: 20,
			speed: 10
		);

		BattleManager.CreateBattleManager(_my, _enemy);

		StartCoroutine(BattleWatching());
		StartCoroutine(EnemyWatch());
		
		SetRpg();
	}

	private IEnumerator BattleWatching()
	{
		while(true)
		{
			if (BattleManager.Instance.IsWin() || BattleManager.Instance.IsLose())
			{
				_battleEnd = true;
				break;
			}

			BattleManager.Instance.Wait();
			
			yield return new WaitForSeconds(WatchTime);
		}
	}

	private IEnumerator EnemyWatch()
	{
		while(true)
		{
			if (IsCommandOk(_enemy))
			{
				BattleManager.Instance.Command(
					new BattleCommand
					{
						ActorId = _enemy.Id,
						ActionCallback = BattleActionLibrary.Attack,
						Target = TargetInfo.CreateTargetId(_my.Id)
					});
			}
			
			yield return new WaitForSeconds(WatchTime);
		}
	}
	
	private void OnGUI()
	{
		int y = 0;
		int height = 50;
		
		string enemyMessage = _enemy.Name + Environment.NewLine + GetHitPointInfo(_enemy) + Environment.NewLine + GetSpeedInfo(_enemy);
		GUI.Box(new Rect(0, height * y, Screen.width, height), enemyMessage);
		y++;
		{
			float progress = (_enemy.Current * 1.0f) / 100;
			GUI.Box(new Rect(0, height * y, Screen.width * progress, height), "");
		}
		y++;
		string myMessage = _my.Name + Environment.NewLine + GetHitPointInfo(_my) + Environment.NewLine + GetSpeedInfo(_my);
		GUI.Box(new Rect(0, height * y, Screen.width, height), myMessage);
		y++;
		{
			float progress = (_my.Current * 1.0f) / 100;
			GUI.Box(new Rect(0, height * y, Screen.width * progress, height), "");
		}
		y++;

		{
			List<GameCommand> commands = CreateCommands();
			_scroll = GUI.BeginScrollView(new Rect(0, height * y, Screen.width, height * 3), _scroll, new Rect(0, 0, Screen.width, height * commands.Count));
			int innerY = 0;

			foreach (var command in commands)
			{
				if (GUI.Button(new Rect(0, innerY * height, Screen.width, height), command.Visual))
				{
					command.Exec();
					break;
				}
				innerY++;
			}
			
			GUI.EndScrollView();
		}
	}

	public override List<GameCommand> CreateCommands()
	{
		List<GameCommand> tmpCommands = new List<GameCommand>();
		
		if (_battleEnd)
		{
			tmpCommands.Add(new GameCommand
			{
				Visual = "タイトルへ",
				Exec = delegate
				{
					SceneManager.LoadScene(SceneConfig.TitleScene);
				}
			});
		}
		
		if (IsCommandOk(_my))
		{
			tmpCommands.Add(new GameCommand
			{
				Visual = "たたかう",
				Exec = (delegate {
					BattleManager.Instance.Command(new BattleCommand
					{
						ActorId = _my.Id,
						ActionCallback = BattleActionLibrary.Attack,
						Target = TargetInfo.CreateTargetId(_enemy.Id)
					});
				})
			});
			
			var items = ItemManager.Instance.Items;
			foreach (var item in items.Values)
			{
				var copiedItem = item;
				tmpCommands.Add(new GameCommand
				{
					Visual = copiedItem.Name,
					Exec = delegate
					{
						BattleManager.Instance.Command(new BattleCommand
						{
							ActorId = _my.Id,
							ActionCallback = copiedItem.Action,
							Target = TargetInfo.CreateTargetId(_my.Id)
						});

						ItemManager.Instance.UseItem(copiedItem);
					}
				});
			}
		}

		return tmpCommands;
	}

	private static string GetHitPointInfo(BattleCharacter character)
	{
		return string.Format("{0} / {1}", character.HitPoint, character.MaxHitPoint);
	}
	
	private static string GetSpeedInfo(BattleCharacter character)
	{
		return string.Format("{0} / {1}", character.Current, 100);
	}
	
	public static bool IsCommandOk(BattleCharacter character)
	{
		return character.Current >= 100;
	}


}
