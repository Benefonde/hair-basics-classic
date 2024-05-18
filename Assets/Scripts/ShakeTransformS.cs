using UnityEngine;
using System.Collections;
 
public class ShakeTransformS : MonoBehaviour
{
   [Header("Info")]
   private Vector3 _startPos;
   private Quaternion _startRot;
   private float _timer;
   private Vector3 _randomPos;
   private Quaternion _randomRot;
 
   [Header("Settings")]
   [Range(0f, 2f)]
   public float _time = 0.2f;
   [Range(0f, 2f)]
   public float _distance = 0.1f;
   [Range(0f, 0.1f)]
   public float _delayBetweenShakes = 0f;
 
   private void Awake()
   {
       _startPos = transform.position;
       _startRot = transform.rotation;
   }
 
   private void OnValidate()
   {
       if (_delayBetweenShakes > _time)
           _delayBetweenShakes = _time;
   }
 
   public void Start()
   {
       StopAllCoroutines();
       StartCoroutine(Shake());
   }

   private Vector3 GetRandomRotations(Vector3 currentRotation)
   {
        float x = transform.rotation.eulerAngles.x + Random.Range(-1f, 1f);
        float y = transform.rotation.eulerAngles.y + Random.Range(-1f, 1f); 
        float z = transform.rotation.eulerAngles.z + Random.Range(-1f, 1f);

        return new Vector3(x, y, z);
   }
 
   private IEnumerator Shake()
   {
       _timer = 0f;
 
       while (_timer < _time)
       {
           _timer += Time.deltaTime;
 
           _randomPos = _startPos + (Random.insideUnitSphere * _distance);
           _randomRot = Quaternion.Euler(GetRandomRotations(transform.rotation.eulerAngles));
 
           transform.position = _randomPos;
           transform.rotation = _randomRot;
 
           if (_delayBetweenShakes > 0f)
           {
               yield return new WaitForSeconds(_delayBetweenShakes);
           }
           else
           {
               yield return null;
           }
       }
 
       transform.position = _startPos;
       transform.rotation = _startRot;
       StartCoroutine(Shake());
   }
}