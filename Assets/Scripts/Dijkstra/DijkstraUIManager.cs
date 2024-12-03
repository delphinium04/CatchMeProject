using System;
using System.Linq;
using Dijkstra.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dijkstra
{
    public class DijkstraUIManager : DijkstraUIBase
    {
        public Action OnResetButtonClicked = null;
        public Action OnConfirmButtonClicked = null;
        public Action OnNextButtonClicked = null;

        enum Texts
        {
            UserPathText,
            TimerText,
            StageText,
            GoalNodeText,
            PlayerText,
            PoliceText,
            ItemText
        }

        enum Buttons
        {
            ResetButton,
            ConfirmButton,
            NextRoundButton,
            ItemCloseButton
        }

        enum GameObjects
        {
            ResultPanel,
            ItemEffectPanel
        }

        void Awake()
        {
            Bind<TMP_Text>(typeof(Texts));
            Bind<Button>(typeof(Buttons));
            Bind<GameObject>(typeof(GameObjects));
        }

        void Start()
        {
            AddButtonListener();
            if (Get<GameObject>((int)GameObjects.ResultPanel) != null)
                Get<GameObject>((int)GameObjects.ResultPanel).SetActive(false);
        }

        void AddButtonListener()
        {
            Button button;
            if ((button = GetButton(Buttons.ResetButton)) != null)
                button.onClick.AddListener(() => OnResetButtonClicked?.Invoke());
            if ((button = GetButton(Buttons.ConfirmButton)) != null)
                button.onClick.AddListener(() => OnConfirmButtonClicked?.Invoke());
            if ((button = GetButton(Buttons.NextRoundButton)) != null)
                button.onClick.AddListener(() => OnNextButtonClicked?.Invoke());
            if ((button = GetButton(Buttons.ItemCloseButton)) != null)
                button.onClick.AddListener(CloseItemAlert);
        }

        TMP_Text GetText(Texts texts) => Get<TMP_Text>((int)texts);
        Button GetButton(Buttons buttons) => Get<Button>((int)buttons);

        public void SetStage(int stageNumber, int targetNode)
        {
            GetText(Texts.StageText).text = $"스테이지 {stageNumber}";
            GetText(Texts.GoalNodeText).text = $": {targetNode}";
        }

        public void SetSpeedText(float thief, float police)
        {
            GetText(Texts.PoliceText).text = $"{police:F1}M/s";
            GetText(Texts.PlayerText).text = $"{thief:F1}M/s";
        }

        public void SetUserPath(params int[] path)
        {
            string text = string.Join(" > ", path);
            GetText(Texts.UserPathText).text = text;
        }

        public void SetTimerText(float t)
        {
            string text = $"Timer: {t:F2}";
            GetText(Texts.TimerText).text = text;
        }

        public void OpenItemAlert(NodeEnum[] nodes)
        {
            Get<GameObject>((int)GameObjects.ItemEffectPanel).SetActive(true);
            GetText(Texts.ItemText).text = "아이템 효과 발동!\n최적 길: ";
            GetText(Texts.ItemText).text += string.Join(" > ", nodes.Select(e => (int)e));
        }

        void CloseItemAlert()
        {
            Get<GameObject>((int)GameObjects.ItemEffectPanel).SetActive(false);
        }

        public void StageClear()
        {
            Get<GameObject>((int)GameObjects.ResultPanel).SetActive(true);
        }
    }
}