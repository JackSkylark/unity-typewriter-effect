using ByteBros.Common.Events;
using ByteBros.Common.Variables;
using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Nodes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ByteBros.Dialog
{
    public class DialogManager : MonoBehaviour
    {
        public StringVariable CurrentActorId;
        public StringVariable DialogLine;
        private Rumor rumor;

        public GameEvent OnNext;
        public GameEvent OnComplete;

        private void Awake()
        {
            rumor = new Rumor(@"
say ""actor1"" ""Hello Gannon! Fancy meeting <i>you</i> here.""
say ""actor2"" ""Well this is a test after all.""
say ""actor3"" ""Hello Everyone!""
say ""actor1"" ""A <i>test</i>!? What do you mean??""
say ""actor2"" ""The creator has chosen us to chat.""
say ""actor2"" ""Please... let us continue our conversation.""
say ""actor1"" ""I think that I'm finished.""
say ""actor1"" ""Good Bye.""
");

            rumor.OnNextNode += (node) =>
            {
                if (node is Say)
                {
                    CurrentActorId.Value = (node as Say).EvaluateSpeaker(rumor).ToString();
                    DialogLine.Value = (node as Say).EvaluateText(rumor);
                    OnNext.Raise();
                }
            };

            rumor.OnFinish += () =>
            {
                OnComplete.Raise();
            };
        }

        private void Start()
        {
            StartCoroutine(rumor.Start());
        }

        public void AdvanceDialog()
        {
            rumor.Advance();
        }

        // Update is called once per frame
        void Update()
        {
            rumor.Update(Time.deltaTime);
        }
    }
}
