using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
	public DynamicInventoryDisplay chestPanel;
	public DynamicInventoryDisplay playerBackpackPanel;
	public PlayerMovement playerMovement;

	private void Awake()
	{
		chestPanel.gameObject.SetActive(false);
		playerBackpackPanel.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
		PlayerInventoryHolder.OnPlayerBackPackDisplayRequested += DisplayPlayerBackpack;
	}

	private void OnDisable()
	{
		InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
		PlayerInventoryHolder.OnPlayerBackPackDisplayRequested -= DisplayPlayerBackpack;
	}

	void Update()
	{
		if (chestPanel.gameObject.activeInHierarchy && Keyboard.current.tabKey.wasPressedThisFrame)
		{
			chestPanel.gameObject.SetActive(false);
			playerMovement.SetCanMove(true);
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		if (playerBackpackPanel.gameObject.activeInHierarchy && Keyboard.current.tabKey.wasPressedThisFrame)
		{
			playerBackpackPanel.gameObject.SetActive(false);
			playerMovement.SetCanMove(true);
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	void DisplayInventory(InventorySystem invToDisplay)
	{
		chestPanel.gameObject.SetActive(true);
		chestPanel.RefreshDynamicInventory(invToDisplay);
		playerMovement.SetCanMove(false);
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}

	void DisplayPlayerBackpack(InventorySystem invToDisplay)
	{
		playerBackpackPanel.gameObject.SetActive(true);
		playerBackpackPanel.RefreshDynamicInventory(invToDisplay);
		playerMovement.SetCanMove(false);
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}
}