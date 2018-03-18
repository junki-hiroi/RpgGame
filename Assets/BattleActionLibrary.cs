using UnityEngine;
using UnityEngine.UI;

public static class BattleActionLibrary
{
    public delegate void BattleAction(BattleCharacter actor, BattleCharacter target);
    
    private static Text _battleText = null;
    
    public static void MessageToText(string message)
    {
        if (_battleText == null)
        {
            var obj = GameObject.FindGameObjectWithTag("BattleText");
            _battleText = obj.GetComponent<Text>();
        }

        if (_battleText == null)
        {
            return;
        }
        
        _battleText.text = message;
    }
    
    public static void Attack(BattleCharacter actor, BattleCharacter target)
    {
        int damage = actor.Attack;
        target.HitPoint = target.HitPoint - actor.Attack;
        MessageToText(string.Format("{0} は {1} に {2} ダメージを与えた.", actor.Name, target.Name, damage));
    }

    public static BattleAction CreateRecoverBattleAction(float power)
    {
        return delegate(BattleCharacter actor, BattleCharacter target) { 
            int recover = (int)(target.MaxHitPoint * power);
            target.HitPoint = target.HitPoint + recover;
            MessageToText(string.Format("{0} は {1} を {2} 回復した.", actor.Name, target.Name, recover));
        };
    }


}
