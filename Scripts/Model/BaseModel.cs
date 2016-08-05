using UnityEngine;
using System.Collections;
using Evolution;

public class BaseModel{

	public int index;
	public ConstEnums.PlayerId ownerId;
	public ConstEnums.Skills cardType;

    public BaseModel()
    {
        ownerId = ConstEnums.PlayerId.None;
        index = -1;
    }

	public void setOwner(ConstEnums.PlayerId playerId){
		ownerId = playerId;
	}

	public void setIndex(int _index){
		index = _index;
	}
}
