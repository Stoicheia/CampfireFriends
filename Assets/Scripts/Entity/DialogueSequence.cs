using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class DialogueSequence
    {
        private List<string> _lines;

        public DialogueSequence(List<string> l)
        {
            _lines = l;
        }

        public DialogueSequence AddLine(string s)
        {
            _lines.Add(s);
            return this;
        }
    }
}