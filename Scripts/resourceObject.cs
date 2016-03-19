using UnityEngine;
using System.Collections;

public class resourceObject {
	public GV.ResourceTypes resType;
	public float quantity;

	public resourceObject(GV.ResourceTypes rT, float quant){
		resType = rT;
		quantity = quant;
	}

	public resourceObject(resourceObject toCopy){
		resType = toCopy.resType;
		quantity = toCopy.quantity;
	}

	public resourceObject give(){
		resourceObject temp = new resourceObject (get());
		zero ();
		return temp;
	}

	public void zero(){
		resType = GV.ResourceTypes.Empty;
		quantity = 0;
	}
}
