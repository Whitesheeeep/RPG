using UnityEngine;

public class UI_Menu : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject SkilltreeUI;
    [SerializeField] private GameObject CraftUI;
    [SerializeField] private GameObject OptionsUI;

    public UI_SkillToolTip skillToolTip;
    public UI_DescriptionToolTip descriptionToolTip;
    public CraftDescpWindow craftDescpWindow;

    private void Start()
    {
        SwitchTo(null);
        descriptionToolTip.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) SwitchWithKeyTO(characterUI);
        if (Input.GetKeyDown(KeyCode.K)) SwitchWithKeyTO(SkilltreeUI);
        if (Input.GetKeyDown(KeyCode.O)) SwitchWithKeyTO(OptionsUI);
        if (Input.GetKeyDown(KeyCode.B)) SwitchWithKeyTO(CraftUI);
    }

    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            _menu.SetActive(true);
        }
    }

    private void SwitchWithKeyTO(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            return;
        }

        SwitchTo(_menu);
    }
}
