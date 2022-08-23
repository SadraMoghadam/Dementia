using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class LightsController : MonoBehaviour
{
    [SerializeField] private GameObject hallwayFirstFloorLightsContainer;
    [SerializeField] private GameObject kitchenLightsContainer;
    [SerializeField] private GameObject diningLightsContainer;
    [SerializeField] private GameObject bathroomWestLightsContainer;
    [SerializeField] private GameObject bathroomEastLightsContainer;
    [SerializeField] private GameObject parkLightsContainer;
    private List<Light> _lights;
    private bool _lightsState;
    private float _timer;

    private void Start()
    {
        _lights = new List<Light>();
        _timer = 0;
    }
    
    public IEnumerator FlickeryLightInPlace(Places place, float time)
    {
        while (_timer < time)
        {
            _timer += Time.fixedDeltaTime;
            TurnLightOfPlaceOnOrOff(place, _lightsState);
            yield return new WaitForSeconds(Random.Range(.1f, 1));
            _lightsState = !_lightsState;
        }
        _timer = 0;
        StopCoroutine(FlickeryLightInPlace(place, time));
    }
    
    public IEnumerator RandomFlickeryLightInPlace(Places place, float time)
    {
        while (_timer < time)
        {
            _timer += Time.fixedDeltaTime;
            _lights = GetLightsOfPlace(place);
            for (int i = 0; i < _lights.Count; i++)
            {
                float rnd = Random.Range(0, 10);
                bool on = rnd < 5 ? true : false;
                _lights[i].gameObject.SetActive(on);
                SetLightsMaterialBrightOrDark(_lights[i], on);
                yield return new WaitForSeconds(Random.Range(.1f, .5f));
            }
        }
        _timer = 0;
        StopCoroutine(RandomFlickeryLightInPlace(place, time));
    }
    
    public void TurnLightOfPlaceOnOrOff(Places place, bool on)
    {
        _lights = GetLightsOfPlace(place);
        for (int i = 0; i < _lights.Count; i++)
        {
            _lights[i].gameObject.SetActive(on);
            SetLightsMaterialBrightOrDark(_lights[i], on);
            // _lights[i].transform.parent.GetComponent<MeshRenderer>().material.
        }
    }
    
    public void TurnLightOfPlacesOnOrOff(List<Places> places, bool on)
    {
        _lights = new List<Light>();
        for (int i = 0; i < places.Count; i++)
        {
            _lights.Concat(GetLightsOfPlace(places[i]));
        }
        for (int i = 0; i < _lights.Count; i++)
        {
            _lights[i].gameObject.SetActive(on);
        }
    }
    
    private List<Light> GetLightsOfPlace(Places place)
    {
        _lights = new List<Light>();
        switch (place)
        {
            case Places.HallwayFirstFloor:
                _lights = hallwayFirstFloorLightsContainer.GetComponentsInChildren<Light>(true).ToList();
                break;
            case Places.KitchenFirstFloor:
                _lights = kitchenLightsContainer.GetComponentsInChildren<Light>(true).ToList();
                break;
            case Places.DiningRoomFirstFloor:
                _lights = diningLightsContainer.GetComponentsInChildren<Light>(true).ToList();
                break;
            case Places.BathRoomFirstFloor:
                _lights = bathroomEastLightsContainer.GetComponentsInChildren<Light>(true).ToList();
                _lights.Concat(bathroomWestLightsContainer.GetComponentsInChildren<Light>(true).ToList());
                break;
            case Places.Park:
                _lights = parkLightsContainer.GetComponentsInChildren<Light>(true).ToList();
                break;
            default:
                break;
        }
        return _lights;
    }
    
    private List<Light> GetAllLights()
    {
        _lights = new List<Light>();
        for (int i = 0; i < Enum.GetValues(typeof(Places)).Length; i++)
        {
            _lights.Concat(GetLightsOfPlace((Places)i));
        }
        return _lights;
    }

    private void SetLightsMaterialBrightOrDark(Light light, bool bright)
    {
        Material mat = light.transform.parent.GetComponent<MeshRenderer>().material;
        if (bright)
        {
            mat.EnableKeyword("_EMISSION");
        }
        else
        {
            mat.DisableKeyword("_EMISSION");
        }
    }


}
