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
            Vanguard,//�O�q
            Midfield,//�^�񒆁H
            RearGuard,//��q
        }
        public enum Phase
        {
            Initialization, //�������t�F�[�Y
            TurnInitialization, //�^�[���J�n���̏������t�F�[�Y
            CommandSelection, //�R�}���h�I���t�F�[
            TargetSelection, //�^�[�Q�b�g�I���t�F�[�Y
            Execution, //���s�t�F�[�Y
            DamageProcessing, //�_���[�W�����t�F�[�Y
            End, //�I���t�F�[�Y
        }
        #region�@�t�B�[���h
        private int mapID; //�}�b�v���ʎq�H
        #endregion

        #region �v���p�e�B
        public int[,] Map;//�@�}�b�v/�W�I���}�ւ̎Q��
        public List<ICharacterData> BattleCharacters;//  �S�퓬�Q���L�����ւ̎Q��
        public List<ICharacterData> AllyCharacters; //  �����w�c�݂̂̎Q��
        public List<ICharacterData> EnemyCharacters;//  �G�w�c�݂̂̎Q��
        public ICharacterData CurrentCharacter; //�s�����L����
        public ICharacterData TargetCharacter; //�^�[�Q�b�g�L����
        public int CurrentTurn=0;//  �^�[���o�ߐ�
        public Phase CurrentPhase; //���݂̃t�F�[�Y

        public string Command { get; set; }

        #endregion

        #region�@���\�b�h
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
         * 1.�}�b�v�̓ǂݍ���
         * 2.�o��L�����N�^�̌���
         * 3.
         */
        private void InitPhase()
        {
            Debug.Log("���݂̃t�F�[�Y�FInit Phase.");

            //�@1�}�b�v�̓ǂݍ���
            Map = new int[3, 4];

            //�@2�o��L�����̌���
            //�����L�����N�^
            AllyCharacters = new()
            {
                new CharacterData
                {
                    Name = "�E��",
                    Description = "���E���~�����ߗ�����E��",
                    HP = 100,
                    MP = 50,
                    Strength = 10,
                    Defense = 5,
                    Speed = 8
                },
                new CharacterData
                {
                    Name = "���@�g��",
                    Description = "���@���g��������|�����@�g��",
                    HP = 80,
                    MP = 80,
                    Strength = 5,
                    Defense = 3,
                    Speed = 6
                },
            };
            //�G�L�����N�^
            EnemyCharacters = new()
            {
                new EnemyCharacter
                {
                    Name = "�X���C��",
                    Description = "�G�������X�^�[",
                    HP = 50,
                    MP = 10,
                    Strength = 5,
                    Defense = 3,
                    Speed = 5
                },
            };
            //�L�����N�^�S�́i�����L����+�G�L�����j
            BattleCharacters = AllyCharacters.Concat(EnemyCharacters).ToList();
            //�^�[���������t�F�[�Y�ֈڍs
            CurrentPhase = Phase.TurnInitialization;
        }

        /*
         * �^�[���������t�F�[�Y
         * 1.�^�[���o�ߐ��̍X�V
         * 2.�s�����L�����̍X�V
         * 3.��Ԉُ�̏���
         * 4.�I�������̃`�F�b�N�i�S��or�����j
         */
        private void TurnInitializationPhase()
        {
            Debug.Log("���݂̃t�F�[�Y�FTurnInit Phase.");

            //�@�^�[���o�ߐ��̍X�V
            CurrentTurn += 1;
            //�s�����L�����̍X�V
            CurrentCharacter = BattleCharacters[CurrentTurn % BattleCharacters.Count];
            //�@��Ԉُ�̏���
            if (CurrentCharacter.AbnormalStatuses != null)
            {
                foreach (var abnormarStatus in CurrentCharacter.AbnormalStatuses)
                {
                    if (abnormarStatus == AbnormalStatus.Poisoned)
                    {
                        //�ł̏����B���^�[��HP�ቺ
                        CurrentCharacter.HP -= 10;
                    }
                    if (abnormarStatus == AbnormalStatus.Paralyzed)
                    {
                        //��Ⴢ̏����B�f�����ቺ
                        CurrentCharacter.Speed -= 10;
                    }
                }
            }
            //�I�������̃`�F�b�N
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
         * �R�}���h�I���t�F�[�Y�B
         */
        private void CommandSelectionPhase()
        {
            Debug.Log("���݂̃t�F�[�Y�FCommand Phase.");

            var temp = CurrentCharacter as IBattleAI;
            if (temp != null)
            {
                Debug.Log("AI���N��:"+CurrentCharacter.Name);

                // AI����R�}���h���擾
                Command = temp.SelectCommand();
            }
            else
            {
                //�@�R�}���h���͂�ҋ@
                Command = "Attack";
            }

            CurrentPhase = Phase.TargetSelection;
        }
        private void TargetSelectionPhase()
        {
            Debug.Log("���݂̃t�F�[�Y�FTarget Phase.");

            //�@�����_���Ń^�[�Q�b�g��I��
            var rand = new System.Random();
            int targetIndex = rand.Next(0, BattleCharacters.Count);
            TargetCharacter = BattleCharacters[targetIndex];

            CurrentPhase = Phase.Execution;
        }
        private void ExecutionPhase()
        {
            Debug.Log("���݂̃t�F�[�Y�FExcecution Phase.");

            Debug.Log($"���s����\nTarget: {TargetCharacter.Name}, Command: {Command}");
            CurrentPhase = Phase.DamageProcessing;
        }
        private void DamageProcessingPhase()
        {
            Debug.Log("���݂̃t�F�[�Y�FDamageProcessing Phase.");
            CurrentPhase = Phase.TurnInitialization;
        }
        private void EndPhase()
        {

        }
        #endregion

    }
}
