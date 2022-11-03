using System;

namespace Models
{
	[Serializable]
	public class Person
	{

		public string FirstName;
		public string LastName;
		public string UserName;
		public string CellPhone;
		public string InviterUsername;
		public int InviterUserID;


		public override string ToString(){
			return UnityEngine.JsonUtility.ToJson (this, true);
		}
	}
}

