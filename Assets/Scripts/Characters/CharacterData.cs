using Assets.Scripts.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

namespace Characters
{
    public interface ICharacterData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public IList<ActionClass> Actions { get; set; }
        // 取得職業（複数化）　レベルのみが各キャラユニーク
        public IList<Job> Jobs { get; set; }
        // 状態異常
        public IList<AbnormalStatus> AbnormalStatuses { get; set; }
        //バフ
        public IList<Buff> Buffs { get; set; }
    }
    // 状態異常
    [Flags]
    public enum AbnormalStatus
    {
        Poisoned, //毒
        Burning, //やけど
        Paralyzed, //麻痺
        Frozen, //凍結
    }

    //　バフ一覧
    [Flags]
    public enum Buff
    { 
        AttackUP,
        AttackDOWN,
        DeffenceUP,
        DeffenceDOWN,
    }
    [Serializable]
    public class CharacterData:ICharacterData
    {
        #region フィールド
        #endregion
        #region プロパティ
        public string Name { get; set; }
        public string Description { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public int Strength  { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public IList<ActionClass> Actions { get; set; }
        // 取得職業（複数化）　レベルのみが各キャラユニーク
        public IList<Job> Jobs { get; set; }
        // 状態異常
        public IList<AbnormalStatus> AbnormalStatuses { get; set; }
        //バフ
        public IList<Buff> Buffs { get; set; }
        #endregion
    }
    public class EnemyCharacter:ICharacterData,IBattleAI
    {
        #region フィールド
        private IList<ActionClass> _actions;
        private IList<Job> _jobs;
        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }

        public IList<ActionClass> Actions { get; set; }
        // 取得職業（複数化）　レベルのみが各キャラユニーク
        public IList<Job> Jobs { get; set; }
        // 状態異常
        public IList<AbnormalStatus> AbnormalStatuses { get; set; }
        //バフ
        public IList<Buff> Buffs { get; set; }
    }
}