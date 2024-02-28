using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarningSceneManager : MonoBehaviour
{
    public float delayInSeconds = 5f;  // 자동으로 넘어갈 딜레이 시간 (초)

    // Start is called before the first frame update
    void Start()
    {
        // 지정된 딜레이 시간 후에 다음 신으로 자동으로 이동하는 코루틴 시작
        StartCoroutine(LoadNextSceneAfterDelay());
    }
    IEnumerator LoadNextSceneAfterDelay()
    {
        // 딜레이 시간만큼 기다린 후에
        yield return new WaitForSeconds(delayInSeconds);

        // 현재 씬의 인덱스를 가져와서 다음 씬이 존재하면 이동

        SceneManager.LoadScene("_3");

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
