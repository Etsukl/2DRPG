using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using System.Linq;
using NUnit;
using static PlasticPipe.PlasticProtocol.Messages.NegotiationCommand;
using static UnityEditor.Sprites.Packer;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.PlayerLoop;
using Codice.Client.Common;
using System.Diagnostics.CodeAnalysis;
using System;

namespace Managers
{
    public class BattleManager : MonoBehaviour
    {

        enum AreaType
        {
            Vanguard,//前衛
            Midfield,//真ん中？
            RearGuard,//後衛
        }
        public enum Phase
        {
            Initialization, //初期化フェーズ
            TurnInitialization, //ターン開始時の初期化フェーズ
            CommandSelection, //コマンド選択フェー
            TargetSelection, //ターゲット選択フェーズ
            Execution, //実行フェーズ
            DamageProcessing, //ダメージ処理フェーズ
            End, //終了フェーズ
        }
        #region　フィールド
        private int mapID; //マップ識別子？
        #endregion

        #region プロパティ
        public int[,] Map;//　マップ/ジオラマへの参照
        public List<ICharacterData> BattleCharacters;//  全戦闘参加キャラへの参照
        public List<ICharacterData> AllyCharacters; //  味方陣営のみの参照
        public List<ICharacterData> EnemyCharacters;//  敵陣営のみの参照
        public ICharacterData CurrentCharacter; //行動中キャラ
        public ICharacterData TargetCharacter; //ターゲットキャラ
        public int CurrentTurn=0;//  ターン経過数
        public Phase CurrentPhase; //現在のフェーズ

        public string Command { get; set; }

        #endregion

        #region　メソッド
        private void Start()
        {
            CurrentPhase = Phase.Initialization;
            InitPhase();
        }
        private void Update()
        {
            switch (CurrentPhase)
            {
                case Phase.Initialization:
                    InitPhase();
                    break;
                case Phase.TurnInitialization:
                    TurnInitializationPhase();
                    break;
                case Phase.CommandSelection:
                    CommandSelectionPhase(); 
                    break;
                case Phase.TargetSelection: 
                    TargetSelectionPhase();
                    break;
                case Phase.Execution:
                    ExecutionPhase();
                    break;
                case Phase.DamageProcessing:
                    DamageProcessingPhase();
                    break;
                case Phase.End:
                    EndPhase();
                    break;
                    
            }
            
        }

        /*
         * 1.マップの読み込み
         * 2.登場キャラクタの決定
         * 3.
         */
        private void InitPhase()
        {
            Debug.Log("現在のフェーズ：Init Phase.");

            //　1マップの読み込み
            Map = new int[3, 4];

            //　2登場キャラの決定
            //味方キャラクタ
            AllyCharacters = new()
            {
                new CharacterData
                {
                    Name = "勇者",
                    Description = "世界を救うため旅する勇者",
                    HP = 100,
                    MP = 50,
                    Strength = 10,
                    Defense = 5,
                    Speed = 8
                },
                new CharacterData
                {
                    Name = "魔法使い",
                    Description = "魔法を使い魔物を倒す魔法使い",
                    HP = 80,
                    MP = 80,
                    Strength = 5,
                    Defense = 3,
                    Speed = 6
                },
            };
            //敵キャラクタ
            EnemyCharacters = new()
            {
                new EnemyCharacter
                {
                    Name = "スライム",
                    Description = "雑魚モンスター",
                    HP = 50,
                    MP = 10,
                    Strength = 5,
                    Defense = 3,
                    Speed = 5
                },
            };
            //キャラクタ全体（味方キャラ+敵キャラ）
            BattleCharacters = AllyCharacters.Concat(EnemyCharacters).ToList();
            //ターン初期化フェーズへ移行
            CurrentPhase = Phase.TurnInitialization;
        }

        /*
         * ターン初期化フェーズ
         * 1.ターン経過数の更新
         * 2.行動中キャラの更新
         * 3.状態異常の処理
         * 4.終了条件のチェック（全滅or勝利）
         */
        private void TurnInitializationPhase()
        {
            Debug.Log("現在のフェーズ：TurnInit Phase.");

            //　ターン経過数の更新
            CurrentTurn += 1;
            //行動中キャラの更新
            CurrentCharacter = BattleCharacters[CurrentTurn % BattleCharacters.Count];
            //　状態異常の処理
            if (CurrentCharacter.AbnormalStatuses != null)
            {
                foreach (var abnormarStatus in CurrentCharacter.AbnormalStatuses)
                {
                    if (abnormarStatus == AbnormalStatus.Poisoned)
                    {
                        //毒の処理。毎ターンHP低下
                        CurrentCharacter.HP -= 10;
                    }
                    if (abnormarStatus == AbnormalStatus.Paralyzed)
                    {
                        //麻痺の処理。素早さ低下
                        CurrentCharacter.Speed -= 10;
                    }
                }
            }
            //終了条件のチェック
            if(AllyCharacters.All(c => c.HP<0) || EnemyCharacters.All(c => c.HP<0))
            {
                CurrentPhase = Phase.End;
            }
            else
            {
                CurrentPhase = Phase.CommandSelection;
            }
        }
        /*
         * コマンド選択フェーズ。
         */
        private void CommandSelectionPhase()
        {
            Debug.Log("現在のフェーズ：Command Phase.");

            var temp = CurrentCharacter as IBattleAI;
            if (temp != null)
            {
                Debug.Log("AIを起動:"+CurrentCharacter.Name);

                // AIからコマンドを取得
                Command = temp.SelectCommand();
            }
            else
            {
                //　コマンド入力を待機
                Command = "Attack";
            }

            CurrentPhase = Phase.TargetSelection;
        }
        private void TargetSelectionPhase()
        {
            Debug.Log("現在のフェーズ：Target Phase.");

            //　ランダムでターゲットを選択
            var rand = new System.Random();
            int targetIndex = rand.Next(0, BattleCharacters.Count);
            TargetCharacter = BattleCharacters[targetIndex];

            CurrentPhase = Phase.Execution;
        }
        private void ExecutionPhase()
        {
            Debug.Log("現在のフェーズ：Excecution Phase.");

            Debug.Log($"実行結果\nTarget: {TargetCharacter.Name}, Command: {Command}");
            CurrentPhase = Phase.DamageProcessing;
        }
        private void DamageProcessingPhase()
        {
            Debug.Log("現在のフェーズ：DamageProcessing Phase.");
            CurrentPhase = Phase.TurnInitialization;
        }
        private void EndPhase()
        {

        }
        #endregion

    }
}
