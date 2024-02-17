using System;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using UnityEngine;

public class PreviewController : MonoBehaviour
{
	public delegate void EditModeEntered();

	public delegate void PartSelected(int partNumber);

	public string arrNameSkin_sett = "arrNameSkin";

	public EditModeEntered editModeEnteredDelegate;

	public PartSelected _partEslected;

	private ArrayList arrNameSkin = new ArrayList();

	public int CurrentTextureIndex;

	public static Vector3 RotationInMuktProfile = new Vector3(17f, 38.28f, 14f);

	public List<StoreKitProduct> _products = new List<StoreKitProduct>();

	public GameObject _purchaseActivityIndicator;

	public GameObject ModelPrefab;

	private Dictionary<int, PanTouchInfo> _panTouches = new Dictionary<int, PanTouchInfo>();

	private Dictionary<int, TapInfo> _tapTouches = new Dictionary<int, TapInfo>();

	private Vector3 rememberedScale;

	private float _scaleModif = 1.25f;

	private float[] bodyYs = new float[2] { 0f, -0.3f };

	private Vector3 _bodyOffset;

	private static string _bodyName = "Body";

	private Vector3 rememberedBodyOffs;

	private float _timeOfLastTapOnChar;

	public bool isEditingMode;

	public bool Locked;

	private GameObject _controller;

	private SpisokSkinov _spisokSkinov;

	private ViborChastiTela _viborChastiTela;

	private bool IsEditingMode
	{
		get
		{
			return !_spisokSkinov.showEnabled;
		}
	}

	public Dictionary<int, PanTouchInfo> PanTouches
	{
		get
		{
			return _panTouches;
		}
	}

	public Dictionary<int, TapInfo> TapTouches
	{
		get
		{
			return _tapTouches;
		}
	}

	public static void SetTextureRecursivelyFrom(GameObject obj, Texture txt)
	{
		foreach (Transform item in obj.transform)
		{
			if ((bool)item.gameObject.GetComponent<Renderer>() && (bool)item.gameObject.GetComponent<Renderer>().material)
			{
				if (item.gameObject.GetComponent<Renderer>().materials.Length > 1)
				{
					Material[] materials = item.gameObject.GetComponent<Renderer>().materials;
					foreach (Material material in materials)
					{
						material.mainTexture = txt;
					}
				}
				else
				{
					item.gameObject.GetComponent<Renderer>().material.mainTexture = txt;
				}
			}
			Texture2D texture2D = (Texture2D)txt;
			texture2D.filterMode = FilterMode.Point;
			SetTextureRecursivelyFrom(item.gameObject, texture2D);
		}
	}

	public void ResetState()
	{
		foreach (TapInfo value in TapTouches.Values)
		{
			Unhighlight(value.TappedCollider.gameObject);
		}
		TapTouches.Clear();
		PanTouches.Clear();
		base.transform.rotation = Quaternion.identity;
	}

	public void ShowSkin(int idx)
	{
		CurrentTextureIndex = idx;
		SetTextureWithIndex(base.gameObject, CurrentTextureIndex);
		ResetState();
	}

	private void Awake()
	{
	}

	public void move(int dir)
	{
		int count = arrNameSkin.Count;
		int num = -1;
		if (dir == 1 * num)
		{
			CurrentTextureIndex++;
			if (CurrentTextureIndex >= count)
			{
				CurrentTextureIndex = 0;
			}
		}
		else
		{
			CurrentTextureIndex--;
			if (CurrentTextureIndex < 0)
			{
				CurrentTextureIndex = count - 1;
			}
		}
		SetTextureWithIndex(base.gameObject, CurrentTextureIndex);
		StartCoroutine(clearAssets());
		ResetState();
		if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 1)
		{
			base.transform.rotation = Quaternion.Euler(RotationInMuktProfile);
		}
		Locked = false;
	}

	private IEnumerator clearAssets()
	{
		Font f = coinsPlashka.thisScript.stLabelCoins.font;
		yield return Resources.UnloadUnusedAssets();
	}

	private void Start()
	{
		_purchaseActivityIndicator = StoreKitEventListener.purchaseActivityInd;
		_purchaseActivityIndicator.SetActive(false);
		if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 1)
		{
			base.transform.rotation = Quaternion.Euler(RotationInMuktProfile);
			CurrentTextureIndex = PlayerPrefs.GetInt(Defs.SkinIndexMultiplayer, 0);
		}
		if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 1)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("InAppGameObject");
			StoreKitEventListener component = gameObject.GetComponent<StoreKitEventListener>();
			_products = component._skinProducts;
		}
		_controller = GameObject.Find("Controller");
		_spisokSkinov = _controller.GetComponent<SpisokSkinov>();
		_viborChastiTela = _controller.GetComponent<ViborChastiTela>();
		updateSpisok();
		if (CurrentTextureIndex >= 0 && arrNameSkin.Count > 0)
		{
			SetTextureRecursivelyFrom(base.gameObject, SkinsManager.TextureForName((string)arrNameSkin[CurrentTextureIndex]));
		}
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager();
		_bodyOffset = GameObject.Find(_bodyName).transform.InverseTransformPoint(new Vector3(0f, bodyYs[1], 0f));
	}

	public void updateSpisok()
	{
		if (arrNameSkin.Count > 0)
		{
			arrNameSkin.Clear();
		}
		string[] c = Load.LoadStringArray(arrNameSkin_sett);
		arrNameSkin.AddRange(c);
	}

	public void Highlight(GameObject go)
	{
		rememberedScale = go.transform.localScale;
		rememberedBodyOffs = Vector3.zero;
		if (go.name.Equals(_bodyName))
		{
			rememberedBodyOffs = go.transform.TransformPoint(_bodyOffset);
		}
		go.transform.localScale *= _scaleModif;
		go.transform.position += rememberedBodyOffs;
	}

	public void Unhighlight(GameObject go)
	{
		go.transform.localScale = rememberedScale;
		if (go.name.Equals(_bodyName))
		{
			go.transform.position -= rememberedBodyOffs;
		}
	}

	public bool TouchOnModel(Touch touch)
	{
		return Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0f)));
	}

	private Collider CheckTap(Touch touch)
	{
		Collider result = null;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0f));
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 100f, -5))
		{
			result = hitInfo.collider;
		}
		return result;
	}

	public void ColliderSelected(Collider collider)
	{
		int partNumber = 0;
		switch (collider.gameObject.name)
		{
		case "Body":
			partNumber = 2;
			break;
		case "Foot_left":
			partNumber = 1;
			break;
		case "Foot_right":
			partNumber = 1;
			break;
		case "Arm_left":
			partNumber = 3;
			break;
		case "Arm_right":
			partNumber = 3;
			break;
		}
		if (_partEslected != null)
		{
			_partEslected(partNumber);
		}
	}

	private void SetTextureWithIndex(GameObject tmpMan, int ind)
	{
		Texture txt = SkinsManager.TextureForName((string)arrNameSkin[ind]);
		SetTextureRecursivelyFrom(tmpMan, txt);
	}

	private void Update()
	{
		_purchaseActivityIndicator.SetActive(StoreKitEventListener.purchaseInProcess);
		if (Locked)
		{
			return;
		}
		float num = 25f;
		float num2 = 35f;
		Rect rect = new Rect(num, num2, (float)Screen.width - num * 2f, (float)Screen.height - num2 * 2f);
		Touch[] touches = Input2.touches;
		for (int i = 0; i < touches.Length; i++)
		{
			Touch touch = touches[i];
			if (touch.phase == TouchPhase.Began)
			{
				if (!rect.Contains(touch.position))
				{
					continue;
				}
				if (TouchOnModel(touch) && IsEditingMode)
				{
					if (TapTouches.Count == 0 && Time.realtimeSinceStartup - _timeOfLastTapOnChar > 1.5f)
					{
						TapInfo tapInfo = new TapInfo();
						tapInfo.TappedCollider = CheckTap(touch);
						if (TapTouches.ContainsKey(touch.fingerId))
						{
							TapTouches.Remove(touch.fingerId);
						}
						TapTouches.Add(touch.fingerId, tapInfo);
						Highlight(tapInfo.TappedCollider.gameObject);
					}
					continue;
				}
				PanTouchInfo panTouchInfo = new PanTouchInfo();
				panTouchInfo.FingerPos = Vector2.zero;
				panTouchInfo.FingerLastPos = Vector2.zero;
				panTouchInfo.FingerMovedBy = Vector2.zero;
				float slideMagnitudeX = (panTouchInfo.SlideMagnitudeY = 0f);
				panTouchInfo.SlideMagnitudeX = slideMagnitudeX;
				panTouchInfo.StartTime = Time.realtimeSinceStartup;
				if (PanTouches.ContainsKey(touch.fingerId))
				{
					PanTouches.Remove(touch.fingerId);
				}
				PanTouches.Add(touch.fingerId, panTouchInfo);
				panTouchInfo.FingerPos = touch.position;
				panTouchInfo.InitialTouchPos = panTouchInfo.FingerPos;
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				if (TapTouches.ContainsKey(touch.fingerId))
				{
					if (!(touch.deltaPosition.magnitude > 10f))
					{
						continue;
					}
					PanTouchInfo panTouchInfo2 = new PanTouchInfo();
					panTouchInfo2.FingerPos = Vector2.zero;
					panTouchInfo2.FingerLastPos = Vector2.zero;
					panTouchInfo2.FingerMovedBy = Vector2.zero;
					float slideMagnitudeX = (panTouchInfo2.SlideMagnitudeY = 0f);
					panTouchInfo2.SlideMagnitudeX = slideMagnitudeX;
					panTouchInfo2.StartTime = Time.realtimeSinceStartup;
					if (PanTouches.ContainsKey(touch.fingerId))
					{
						PanTouches.Remove(touch.fingerId);
					}
					PanTouches.Add(touch.fingerId, panTouchInfo2);
					panTouchInfo2.FingerPos = touch.position - touch.deltaPosition;
					panTouchInfo2.InitialTouchPos = panTouchInfo2.FingerPos;
					Unhighlight(TapTouches[touch.fingerId].TappedCollider.gameObject);
					TapTouches.Remove(touch.fingerId);
				}
				if (PanTouches.ContainsKey(touch.fingerId))
				{
					PanTouches[touch.fingerId].FingerMovedBy = touch.position - PanTouches[touch.fingerId].FingerPos;
					PanTouches[touch.fingerId].FingerLastPos = PanTouches[touch.fingerId].FingerPos;
					PanTouches[touch.fingerId].FingerPos = touch.position;
					PanTouches[touch.fingerId].SlideMagnitudeX = PanTouches[touch.fingerId].FingerMovedBy.x;
					PanTouches[touch.fingerId].SlideMagnitudeY = PanTouches[touch.fingerId].FingerMovedBy.y;
					if (IsEditingMode)
					{
						float num5 = 0.5f;
						base.gameObject.transform.Rotate(0f, (0f - num5) * PanTouches[touch.fingerId].SlideMagnitudeX, 0f, Space.World);
					}
				}
			}
			else if (touch.phase == TouchPhase.Stationary)
			{
				if (!TapTouches.ContainsKey(touch.fingerId) && PanTouches.ContainsKey(touch.fingerId))
				{
					PanTouches[touch.fingerId].FingerLastPos = PanTouches[touch.fingerId].FingerPos;
					PanTouches[touch.fingerId].FingerPos = touch.position;
					PanTouches[touch.fingerId].SlideMagnitudeX = 0f;
					PanTouches[touch.fingerId].SlideMagnitudeY = 0f;
				}
			}
			else
			{
				if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
				{
					continue;
				}
				if (TapTouches.ContainsKey(touch.fingerId))
				{
					if (touch.phase == TouchPhase.Ended)
					{
						ColliderSelected(TapTouches[touch.fingerId].TappedCollider);
					}
					Unhighlight(TapTouches[touch.fingerId].TappedCollider.gameObject);
					TapTouches.Remove(touch.fingerId);
				}
				else
				{
					if (!PanTouches.ContainsKey(touch.fingerId))
					{
						continue;
					}
					if (!IsEditingMode && touch.phase == TouchPhase.Ended)
					{
						if (TouchOnModel(touch) && (touch.position - PanTouches[touch.fingerId].InitialTouchPos).magnitude < 15f && Time.realtimeSinceStartup - PanTouches[touch.fingerId].StartTime < 0.45f)
						{
							if (editModeEnteredDelegate != null)
							{
								editModeEnteredDelegate();
							}
							ResetState();
							break;
						}
						if (Mathf.Abs((PanTouches[touch.fingerId].InitialTouchPos - touch.position).x) > (float)(Screen.width / 10) && !(Time.realtimeSinceStartup - PanTouches[touch.fingerId].StartTime < 2.5f))
						{
						}
					}
					PanTouches.Remove(touch.fingerId);
				}
			}
		}
	}

	public void TestDelegate(int pn)
	{
		Debug.Log(pn);
	}

	private void SetProducts(List<StoreKitProduct> allProducts)
	{
		_products = allProducts;
		Debug.Log("All Products: ");
		for (int i = 0; i < _products.Count; i++)
		{
			Debug.Log(_products[i].productIdentifier + "\n");
		}
	}

	private void OnEnable()
	{
		StoreKitManager.productListReceivedEvent += SetProducts;
		StoreKitManager.purchaseSuccessfulEvent += purchaseSuccessful;
		StoreKitManager.purchaseCancelledEvent += purchaseCancelled;
		StoreKitManager.purchaseFailedEvent += purchaseFailed;
	}

	private void OnDisable()
	{
		StoreKitManager.productListReceivedEvent -= SetProducts;
		StoreKitManager.purchaseSuccessfulEvent -= purchaseSuccessful;
		StoreKitManager.purchaseCancelledEvent -= purchaseCancelled;
		StoreKitManager.purchaseFailedEvent -= purchaseFailed;
	}

	private void purchaseSuccessful(StoreKitTransaction tr)
	{
		PurchaseSuccessful(tr.productIdentifier);
	}

	public void PurchaseSuccessful(string id)
	{
		if (VirtualCurrencyHelper.prices[id] > keychainPlugin.getKCValue(Defs.Coins))
		{
			return;
		}

		keychainPlugin.updateKCValue(keychainPlugin.getKCValue(Defs.Coins) - VirtualCurrencyHelper.prices[id], Defs.Coins);
		
		if (Array.IndexOf(StoreKitEventListener.skinIDs, id) >= 0)
		{
			Storager.setInt(InAppData.inAppData[CurrentTextureIndex].Value, 1);
		}
		Locked = false;
	}

	private void purchaseCancelled(string err)
	{
		Locked = false;
	}

	private void purchaseFailed(string err)
	{
		Locked = false;
	}
}
