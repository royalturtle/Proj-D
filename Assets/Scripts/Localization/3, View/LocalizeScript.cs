using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizeScript : MonoBehaviour {
    [SerializeField] LocalizationTypes Type;

    public int textKey;      //0��° ��(���� ������)�� �������� key�� ���� ���ڿ� ���� 
    public int[] dropdownKey;    //���� text�� �ƴ� ����ٿ��� ���
    public bool IsNewLine = true;

    private void Start() {
        LocalizeChanged();
        LanguageSingleton.Instance.LocalizeChanged += LocalizeChanged;
    }

    private void OnDestroy() {
        LanguageSingleton.Instance.LocalizeChanged -= LocalizeChanged;
    }

    //� ���ڿ��� �ʿ����� key�� �Ű������� �޴´� 
    // string Localize(string key)
    protected string Localize(int key) {
        // int keyIndex = LanguageSingleton.Instance.Langs[0].value.FindIndex(x => x.ToLower() == key.ToLower()); //������ �Ǵ� 0�� �������� value Ž�� �� keyIndex�� ���ڿ� ����
        return LanguageSingleton.Instance.Data[Type][key];      //���� ��� �ε���, value�� key�� �������� ���ڿ��� ��ȯ�Ѵ�. 
    }

    protected virtual void LocalizeChanged() {
        if (GetComponent<TextMeshProUGUI>() != null) {
            GetComponent<TextMeshProUGUI>().text = IsNewLine ? Localize(textKey).Replace("\\n", "\n") : Localize(textKey);
        }

        else if (GetComponent<TMP_Dropdown>() != null) {
            TMP_Dropdown dropdown = GetComponent<TMP_Dropdown>();
            dropdown.captionText.text = Localize(dropdownKey[dropdown.value]);

            for (int i = 0; i < dropdown.options.Count; i++) {
                dropdown.options[i].text = Localize(dropdownKey[i]);
            }
        }
    }
}