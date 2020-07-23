using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondUse : OnUseItem
{
	public override bool hit(Item item){
    	return false;
    }
}