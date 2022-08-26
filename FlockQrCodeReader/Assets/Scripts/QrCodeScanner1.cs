using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using TMPro;
using UnityEngine.UI;

public class QRCodeScanner : MonoBehaviour {
	
	[SerializeField]
	private RawImage _rawImageBackground;
	[SerializeField]
	private AspectRatioFitter _aspectRatioFitter;
	[SerializeField]
	private TextMeshProUGUI _textOut;
	[SerializeField]
	private RectTransform _scanZone;

	private bool _isCamAvailable;
	private WebCamTexture _cameratexture;

	// Start is called before the first frame update
	void Start() {
		
	}
	
	void Update () {
		UpdateCameraRender();
	}
	
	void SetupCamera() {

		WebCamDevice[] devices = WebCamTexture.devices;
		
		if(devices.Length==0) {
			_isCamAvailable = false;
			return;
		}
		
		for( int i=0; i<devices.Length; i++) {
			if(devices[i].isFrontFacing==false) {
				_cameratexture = WebCamTexture(devices[i].name, (int)_scanZone.rect.width, (int)_scanZone.rect.height);
			}
		}
		
		_cameratexture.Play();
		_rawImageBackground.texture = _cameratexture;
		_isCamAvailable = true;
		
	}

	private void UpdateCameraRender() {
		
		if(_isCamAvailable==false) {
			return;
		}
		
		float ratio = (Float)_cameratexture.witdh / (float)_cameratexture.height;
		_aspectRatioFitter.aspectRatio = ratio;
		
		int orientation = _cameratexture.videoRotationAngle;
		_rawImageBackground.rectTransform.localEulerAngles = new Vector3(0,0,orientation);
	}
	public void OnClickScan() {
		
		Scan();
	
	}
	
	private void Scan() {
		
		try {
			IBarcodeReader barcodeReader = BarcodeReader();
			Result result = barcodeReader.Decode(_cameratexture.GetPixels32(),_cameratexture.witdh, _cameratexture.height);
			if( result != null) {
				_textOut.text = result.Text;
			} else {
				_textOut.text = "Error al leer el QR";
			}
		} catch(Exception ex) {
			_textOut.text = "Excepción al leer el QR";
		}
	}
}