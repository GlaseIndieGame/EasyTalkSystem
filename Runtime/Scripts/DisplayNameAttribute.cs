using UnityEngine;

namespace EasyTalkSystem
{
    public class DisplayNameAttribute : PropertyAttribute
    {
        public string DisplayName { get; private set; }

        public DisplayNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
