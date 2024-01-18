using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{

    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;
    public GameObject brokeTextPrefab;
    public GameObject numPanel;

    private void Awake()
    {
    
    }

    private void OnEnable()
    {
        CharacterEvent.characterDamaged += CharacterTookDamage;
        CharacterEvent.characterHealed += CharacterTookHeal;
        CharacterEvent.characterBroken += CharacterGetBroke;

    }

    private void OnDisable()
    {
        CharacterEvent.characterDamaged -= CharacterTookDamage;
        CharacterEvent.characterHealed -= CharacterTookHeal;
        CharacterEvent.characterBroken -= CharacterGetBroke;
    }

    // Start is called before the first frame update
    public void CharacterTookDamage(GameObject character, int damageReceived)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        spawnPosition = new Vector3(Random.Range(-20, 20) + spawnPosition.x, Random.Range(-20, 20) + spawnPosition.y, spawnPosition.z);

        TMP_Text tMPText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity).GetComponent<TMP_Text>();
        tMPText.transform.SetParent(numPanel.transform, false);
        tMPText.transform.localScale = new Vector3(1, 1, 1);

        tMPText.text = damageReceived.ToString();
    }

    public void CharacterTookHeal(GameObject character, int healReceived)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        spawnPosition = new Vector3(Random.Range(-20, 20) + spawnPosition.x, Random.Range(-20, 20) + spawnPosition.y, spawnPosition.z);
        TMP_Text tMPText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity).GetComponent<TMP_Text>();
        tMPText.transform.SetParent(numPanel.transform, false);
        tMPText.transform.localScale = new Vector3(1, 1, 1);

        tMPText.text = healReceived.ToString();
    }

    public void CharacterGetBroke(GameObject character)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        spawnPosition = new Vector3(Random.Range(-20, 20) + spawnPosition.x, Random.Range(-20, 20) + spawnPosition.y, spawnPosition.z);
        TMP_Text tMPText = Instantiate(brokeTextPrefab, spawnPosition, Quaternion.identity).GetComponent<TMP_Text>();
        tMPText.transform.SetParent(numPanel.transform, false);
        tMPText.transform.localScale = new Vector3(1, 1, 1);

        tMPText.text = "broken";
    }


    public void OnExitGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {

        }
    }
}
