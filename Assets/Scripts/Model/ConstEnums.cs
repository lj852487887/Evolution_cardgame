using UnityEngine;
using System;
using System.Collections;

namespace Evolution{
	
	public class ConstEnums {

		public static int ANIMAL_HAS_SKILL = -2;
		public static int NOT_ON_Animal = -1;
		
		public enum PlayerId{
			None,
			First,
			Second,
			Third,
			Fourth
		}

		public enum GameState{
			None,
			Evolute,
			Hunt,
			End
		}

		public enum Food {
			Fat,
			Meat
		}

		public enum Skills{
			None,
			Fat,
			Predator,
			Hide,
			Aquatic
		}

        static public int getSkillCount()
        {
            return System.Enum.GetValues(typeof(Skills)).Length;
        }

	}

}

