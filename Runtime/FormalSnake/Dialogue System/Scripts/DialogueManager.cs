using static FormalSnake.DialogueSystem.DialogueSystem;
using UnityEngine.UI;
using UnityEngine;
using System;
namespace FormalSnake.DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        public DialogueSystem dialogueSystem;
        public GameObject buttonPrefab;

        private DialogueNode currentNode;
        private GameObject dialogueUI;

        public void StartDialogue()
        {
            currentNode = dialogueSystem.startNode;

            // create the dialogue UI
            dialogueUI = new GameObject("Dialogue UI");
            Canvas canvas = dialogueUI.AddComponent<Canvas>();
            CanvasScaler scaler = dialogueUI.AddComponent<CanvasScaler>();
            GraphicRaycaster graphicRaycaster = dialogueUI.AddComponent<GraphicRaycaster>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            scaler.scaleFactor = 1;
            scaler.referencePixelsPerUnit = 100;
            graphicRaycaster.ignoreReversedGraphics = true;
            graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1;

            Text dialogueText = new GameObject("Dialogue Text").AddComponent<Text>();
            dialogueText.transform.SetParent(canvas.transform);
            dialogueText.text = currentNode.dialogue;
            dialogueText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            dialogueText.fontSize = 24;
            dialogueText.alignment = TextAnchor.MiddleCenter;

            VerticalLayoutGroup layout = canvas.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.spacing = 10;

            for (int i = 0; i < currentNode.choices.Count; i++)
            {
                Choice choice = currentNode.choices[i];
                GameObject button = Instantiate(buttonPrefab, canvas.transform);
                button.tag = "ChoiceButton";
                button.GetComponentInChildren<Text>().text = choice.choiceText;

                int choiceIndex = i;
                button.GetComponent<Button>().onClick.AddListener(() => OnChoiceSelected(choiceIndex));

            }
        }

        void OnChoiceSelected(int choiceIndex)
        {
            Debug.Log(dialogueUI);
            currentNode = dialogueSystem.GetNextNode(currentNode, choiceIndex);

            if (currentNode == null)
            {
                // end the dialogue
                return;
            }

            // update the dialogue text and choices
            dialogueUI.GetComponentInChildren<Text>().text = currentNode.dialogue;

            // remove the old buttons
            foreach (Transform child in dialogueUI.transform)
            {
                if (child.CompareTag("ChoiceButton"))
                {
                    Destroy(child.gameObject);
                }
            }

            // create new buttons for the current node's choices and add them to the canvas
            for (int i = 0; i < currentNode.choices.Count; i++)
            {
                Choice choice = currentNode.choices[i];
                GameObject button = Instantiate(buttonPrefab, dialogueUI.transform);
                button.GetComponentInChildren<Text>().text = choice.choiceText;
                button.tag = "ChoiceButton";

                int index = i;
                button.GetComponent<Button>().onClick.AddListener(() => OnChoiceSelected(index));
            }
            if (currentNode == null)
            {
                // end the dialogue
                if (dialogueUI != null)
                {
                    Destroy(dialogueUI);
                }
                return;
            }
            if (currentNode.isLast)
            {
                Destroy(dialogueUI);
            }
        }


    }
}