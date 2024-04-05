using UnityEngine;

namespace FruitNinja.Scripts
{
    public class G80_FrozenFruit : G80_Fruit
    {

        [SerializeField] private float slowDuration;
        [SerializeField] private float slowScale;

        public override void OnSliced(Vector3 sliceDirection, Vector3 positionContact, float force)
        {
            base.OnSliced(sliceDirection, positionContact, force);
            StartCoroutine(G80_GameManager.Instance.EnterSlowMotion(slowDuration, slowScale));
        }
    }
}