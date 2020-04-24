using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;

public class RAController : MonoBehaviour
{
    public GameObject nextButton, previousButton, point;

    public GameObject[] cars;

    public GameObject[] cars_clone;

    int index = 0;

    public Text brandLabel, modelLabel;

    JSONNode dataCars;

    // Start is called before the first frame update
    void Start()
    {
        cars_clone = new GameObject[cars.Length];
        for(int i = 0; i <= cars.Length - 1; i++){
            GameObject car = Instantiate(cars[i], new Vector3(0, 0, 0), Quaternion.identity);
            car.transform.parent = point.transform;
            car.transform.localPosition = new Vector3(0, 0, 0);
            car.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            car.name = i.ToString();
            car.SetActive(false);
            cars_clone[i] = car;
        }
        ReadJson();
    }

    // Update is called once per frame
    void Update()
    {
        // Show and hide cars
        for(int i=0; i <= cars_clone.Length - 1; i++){
            if(cars_clone[i].name == index.ToString()){
                cars_clone[i].SetActive(true);
                brandLabel.text = dataCars[i.ToString()]["brand"];
                modelLabel.text = dataCars[i.ToString()]["model"];
            } else {
                cars_clone[i].SetActive(false);
            }
        }

        
        if(index == 0){
            previousButton.SetActive(false);
        } else if( index == cars.Length - 1 ){
            nextButton.SetActive(false);
        } else {
            nextButton.SetActive(true);
            previousButton.SetActive(true);
        }
    }

    public void Next(){
        index += 1;
    }

    public void Previous(){
        index -= 1;
    }

    public void ChangeColor(string hex_color){
        Color color = GetColorRGBA(hex_color);

        foreach(Transform child in cars_clone[index].transform){
            if(child.name == "body"){
                Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                mat.SetColor("_BaseColor", color);
                child.gameObject.GetComponent<Renderer>().material = mat;
            }
        }
    }

    Color GetColorRGBA(string hex_color){
        Color new_color;
        ColorUtility.TryParseHtmlString(hex_color, out new_color);
        return new_color;
    }

    void ReadJson(){
        string data = Resources.Load<TextAsset>("cars").text;
        dataCars = JSONNode.Parse(data)["cars"];
    }
}
