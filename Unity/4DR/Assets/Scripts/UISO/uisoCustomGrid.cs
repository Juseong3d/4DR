using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uisoCustomGrid : MonoBehaviour
{

    public UIGrid[] _grids;
    public GameObject _char;

    // Start is called before the first frame update
    void Start()
    {
        

        for(int i = 0; i<16; i++) {
            GameObject _obj = new GameObject();
            UIGrid addgrid = _obj.AddComponent<UIGrid>();

            addgrid.cellWidth = 100;
            addgrid.cellHeight = 100;
            addgrid.pivot = UIWidget.Pivot.Center;
            addgrid.name = string.Format("{0:00}_{1:00}", i, i + 5);

            _obj.transform.SetParent(this.transform);
            _obj.transform.localScale = new Vector3(1, 1, 1);

            for(int j = 0; j<(i + 5); j++) {
                GameObject _charNum = Appimg._INSTANTIATE4UI(_char);

                _charNum.transform.SetParent(_obj.transform);
                _charNum.transform.localScale = new Vector3(1, 1, 1);

                UILabel label = _charNum.GetComponentInChildren<UILabel>();

                label.text = i + "_" + j;
            }

            addgrid.repositionNow = true;
        }

        _grids = GetComponentsInChildren<UIGrid>();

        _grids[0].repositionNow = true;
                
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
