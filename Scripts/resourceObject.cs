using UnityEngine;
using System.Collections;

public class resourceObject {
	public GV.ResourceTypes resType;
	public float quantity;
	public bool isPoison;

	public resourceObject(GV.ResourceTypes rT, float quant, bool isPois){
		resType = rT;
		quantity = quant;
		isPoison = isPois;
	}

	public resourceObject(resourceObject toCopy){
		resType = toCopy.resType;
		quantity = toCopy.quantity;
		isPoison = toCopy.isPoison;
	}

	public resourceObject give(){
		resourceObject temp = new resourceObject (this);
		zero ();
		return temp;
	}

	public void zero(){
		resType = GV.ResourceTypes.Empty;
		quantity = 0;
		isPoison = false;
	}

	public bool isZero(){
		return quantity == 0 && resType == GV.ResourceTypes.Empty;
	}

	public bool consume(float amount){
		if(amount > quantity)
			return false;

		amount -= quantity;
		//DO SOME OTHER THING? OR IS IT HANDLE BY CALLER? << PROB LAST ONE
		return true;
	}
}
