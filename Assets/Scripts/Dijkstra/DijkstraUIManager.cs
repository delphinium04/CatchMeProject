using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            PoliceText
        }

        enum Buttons
        {
            ResetButton,
            ConfirmButton,
            NextRoundButton,
        }

        enum GameObjects
        {
            ResultPanel,
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
        }

        TMP_Text GetText(Texts texts) => Get<TMP_Text>((int)texts);
        Button GetButton(Buttons buttons) => Get<Button>((int)buttons);

        public void SetStage(int stageNumber, int targetNode)
        {
            GetText(Texts.StageText).text = $"스테이지 {stageNumber}";
            GetText(Texts.GoalNodeText).text = $": {targetNode}";
        }

        public void SetUserPath(params int[] path)
        {
            string text = String.Join(" > ", path);
            GetText(Texts.UserPathText).text = text;
        }

        public void SetTimerText(float t)
        {
            string text = $"Timer: {t:F2}";
            GetText(Texts.TimerText).text = text;
        }

        public void StageClear()
        {
            Get<GameObject>((int)GameObjects.ResultPanel).SetActive(true);
        }
    }
}