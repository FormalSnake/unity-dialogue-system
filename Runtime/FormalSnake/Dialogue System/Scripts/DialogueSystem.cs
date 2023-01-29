using UnityEngine;
using System.Collections.Generic;
using static FormalSnake.DialogueSystem.DialogueSystem;
using System;

/*
DialogueSystem is a scriptable object that allows for easy creation and management of dialogue choices.
It can be used to create dialogue trees and branching storylines.
*/
namespace FormalSnake.DialogueSystem
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "FormalSnake/Dialogue", order = 1)]

    public class DialogueSystem : ScriptableObject
    {
        [System.Serializable]
        public class DialogueNode
        {
            public string dialogue;
            public List<Choice> choices;
            public bool isLast;

            // constructor for DialogueNode, initializes dialogue and choice list
            public DialogueNode(string dialogue)
            {
                this.dialogue = dialogue;
                choices = new List<Choice>();
            }
        }

        [System.Serializable]
        public class Choice
        {
            public string choiceText;
            public DialogueNode nextNode;

            // constructor for Choice, initializes choice text and the next node in the dialogue
            public Choice(string choiceText, DialogueNode nextNode)
            {
                this.choiceText = choiceText;
                this.nextNode = nextNode;
            }
        }

        public DialogueNode startNode;

        public DialogueNode GetNextNode(DialogueNode currentNode, int choiceIndex)
        {
            return currentNode.choices[choiceIndex].nextNode;
        }

        public List<string> GetChoices(DialogueNode currentNode)
        {
            List<string> choiceTexts = new List<string>();
            foreach (Choice choice in currentNode.choices)
            {
                choiceTexts.Add(choice.choiceText);
            }
            return choiceTexts;
        }

    }
}