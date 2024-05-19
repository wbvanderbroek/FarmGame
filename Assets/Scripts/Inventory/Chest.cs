using UnityEngine;

public class Chest : MonoBehaviour
{
    public InventoryObject chest;
    public GameObject chestUI;
    public int ChestID = 0;
    private Vector3 currentAngle;
    private Vector3 targetAngle = new Vector3(0, 0, 0);

    private void Start()
    {
        currentAngle = transform.GetChild(0).localEulerAngles;
        chest.CreateFile(ChestID);
    }
    public void OpenChest()
    {
        chestUI.SetActive(true);

        chest.Load(ChestID);
        currentAngle = transform.GetChild(0).localEulerAngles;
        targetAngle = new Vector3(-40, 0, 0);
    }
    public void CloseChest()
    {
        chestUI.SetActive(false);

        chest.Save(ChestID);
        currentAngle = transform.GetChild(0).localEulerAngles;
        targetAngle = new Vector3(0, 0, 0);

    }
    private void Update()
    {
        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime * 3),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime * 3),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime * 3));
        transform.GetChild(0).localEulerAngles = currentAngle;
    }

    private void OnApplicationQuit()
    {
        //chest.Save(ChestID);
        //chest.Container.Clear();
    }
}
