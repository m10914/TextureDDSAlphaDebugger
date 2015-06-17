using UnityEngine;
using System.Collections;

public class PlaneBeforeCamera : MonoBehaviour {

	public Material mat;
	public float clipValue;

	private GameObject plane;
	private bool bClip = false;	
		
	private float sliderValue;
	private float alpha;

	private Vector3 pos;
	private bool bLMB = false;


	// Use this for initialization
	void Start () 
	{	
		//load texture
		byte[] bytes = System.IO.File.ReadAllBytes("./texture.dds");
		Texture2D tex = LoadTextureDXT(bytes, TextureFormat.DXT5);
		mat.mainTexture = tex;

		// setup everything
		plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		plane.transform.eulerAngles = new Vector3(-90,0,0);
		plane.transform.position = new Vector3(0,0.8f,0);

		plane.GetComponent<Renderer>().material = mat;

		pos = new Vector3(0, 0.8f, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{	
		mat.SetFloat("clipValue", sliderValue);
		mat.SetFloat("alpha", bClip ? 1 : 0);

		plane.transform.position = new Vector3(pos.x, pos.y, pos.z);

		if(Input.GetMouseButtonDown(0))
		{
			bLMB = true;
		}
		else if(Input.GetMouseButtonUp(0))
		{
			bLMB = false;
		}

		if(Input.mousePosition.y < Screen.height - 150)
		{
			float speed = 0.5f;
			if(bLMB)
			{
				float dx = Input.GetAxis ("Mouse X") * speed;
				float dy = Input.GetAxis ("Mouse Y") * speed;

				pos += new Vector3(dx,dy,0);
			}

			float dz = Input.GetAxis("Mouse ScrollWheel") * speed;
			pos.z += dz * 6;
		}


	}

	void OnGUI () {

		GUI.Box (new Rect(10,10,150,150), "Settings menu");

		if (GUI.Button(new Rect(10,30,140,20), bClip ? "turn blend off" : "turn blend on"))
		{
			bClip = !bClip;
		}

		sliderValue = GUI.HorizontalSlider(new Rect(15, 55, 440, 15), sliderValue, 0.0f, 1.0f);

		string toEdit = "" + sliderValue;
		toEdit = GUI.TextField (new Rect (10, 75, 50, 20), toEdit, 25);
		float.TryParse(toEdit, out sliderValue);
		sliderValue = Mathf.Round(sliderValue * 1000) / 1000;
	}


	//------------------------------------------------------------------------------------
	public static Texture2D LoadTextureDXT(byte[] ddsBytes, TextureFormat textureFormat)
	{
		if (textureFormat != TextureFormat.DXT1 && textureFormat != TextureFormat.DXT5)
			return null;
			//throw new Exception("Invalid TextureFormat. Only DXT1 and DXT5 formats are supported by this method.");
		
		byte ddsSizeCheck = ddsBytes[4];
		if (ddsSizeCheck != 124)
			return null;
			//throw new Exception("Invalid DDS DXTn texture. Unable to read");  //this header byte should be 124 for DDS image files
		
		int height = ddsBytes[13] * 256 + ddsBytes[12];
		int width = ddsBytes[17] * 256 + ddsBytes[16];
		
		int DDS_HEADER_SIZE = 128;
		byte[] dxtBytes = new byte[ddsBytes.Length - DDS_HEADER_SIZE];
		System.Buffer.BlockCopy(ddsBytes, DDS_HEADER_SIZE, dxtBytes, 0, ddsBytes.Length - DDS_HEADER_SIZE);
		
		Texture2D texture = new Texture2D(width, height, textureFormat, false);
		texture.LoadRawTextureData(dxtBytes);
		texture.Apply();
		
		return (texture);
	}
	
}
