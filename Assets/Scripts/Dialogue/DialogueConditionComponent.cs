using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueConditionComponent : DialogueComponent
{
        [System.Serializable]
        public class DialogueOption
        {
            public VariableManager.Condition[] Conditions;
            public DialogueScriptSO Cutscene;
        }
        public DialogueOption[] options;

        public override void Play()
        {
            foreach (var option in options)
            {
                if (GameController.main.variables.AllConditionsMet(option.Conditions))
                {
                    UIController.main.dialogueController.PlayCutscene(option.Cutscene);
                    return;
                }
            }
            base.Play();
        
    }

}
