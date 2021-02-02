using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class _Timer : MonoBehaviour
{
    public Text _time;
    float _timer = 0;
    int min = 0;

    private void Start()
    {
        _time = GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= 60)
        {
            min++;
            _timer -= 60;
        }

        _time.text = string.Format("{0:D2} : {1:D2}", min, (int)_timer);

    }

    private void OnDisable()
    {
        _time.text = "00 : 00";
        _timer = 0;
        min = 0;
    }
}
