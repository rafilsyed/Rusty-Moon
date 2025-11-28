using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DynamicInventoryDisplay : InventoryDisplay
{
	[SerializeField]
	protected InventorySlot_UI slotPrefab;

	protected override void Start()
	{
		base.Start();
	}

	public void RefreshDynamicInventory(InventorySystem invToDisplay)
	{
		ClearSlots();
		inventorySystem = invToDisplay;
		if (inventorySystem != null) inventorySystem.OnInventorySlotChanged += UpdateSlot;
		AssignSlot(invToDisplay);
	}

	public override void AssignSlot(InventorySystem invToDisplay)
	{
		ClearSlots();
		slotDictonary = new Dictionary<InventorySlot_UI, InventorySlot>();

		if (invToDisplay == null) return;

		for (int i = 0; i < invToDisplay.InventorySize; i++)
		{
			var uiSlot = Instantiate(slotPrefab, transform);
			slotDictonary.Add(uiSlot, invToDisplay.InventorySlots[i]);
			uiSlot.Init(invToDisplay.InventorySlots[i]);
			uiSlot.UpdateUISlot();
		}
	}

	private void ClearSlots()
	{
		foreach (var item in transform.Cast<Transform>())
		{
			Destroy(item.gameObject);
		}

		if (slotDictonary != null)
			slotDictonary.Clear();
	}

	private void OnDisable()
	{
		if (inventorySystem != null) inventorySystem.OnInventorySlotChanged -= UpdateSlot;
	}
}