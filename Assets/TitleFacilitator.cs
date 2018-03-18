using System.Collections.Generic;
using RunTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleFacilitator : RpgBehaviour
{
    private Vector2 _scroll;

    void Start()
    {
        SetRpg();
    }

    private void OnGUI()
    {
        int y = 0;
        int height = 50;

        GUI.Box(new Rect(0, height * y, Screen.width, height), "タイトル");
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

        tmpCommands.Add(new GameCommand
        {
            Visual = "バトル開始",
            Exec = delegate
            {
                SceneManager.LoadScene(SceneConfig.BattleScene);
            }
        });

        tmpCommands.Add(new GameCommand
        {
            Visual = "ショップ",
            Exec = delegate
            {
                SceneManager.LoadScene(SceneConfig.ShopScene);
            }
        });

        return tmpCommands;
    }
}
