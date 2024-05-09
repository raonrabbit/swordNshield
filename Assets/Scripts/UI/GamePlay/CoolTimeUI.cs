using UnityEngine;
using UnityEngine.UI;

namespace SwordNShield.UI
{
    public class CoolTimeUI : MonoBehaviour
    {
        private static CoolTimeUI instance = null;
        public Image QSkillImage;
        public Image WSkillImage;
        public Image ESkillImage;
        public Image RSkillImage;
        public Image DSkillImage;
        public Image FSkillImage;

        void Awake(){
            if(instance == null) instance = this;
            else Destroy(this.gameObject);
        }

        public static CoolTimeUI Instance{
            get => instance;
        }
    }
}
