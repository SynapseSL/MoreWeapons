using UnityEngine;

namespace MoreWeapons.Scripts
{
    public class StickyComponent : MonoBehaviour
    {
        public GameObject owner;
        public Grenades.Grenade grenade;

        public Vector3 relativePosition;
        public GameObject collider = null;

        public void OnCollisionEnter(Collision col)
        {
            if (collider != null || col.gameObject == owner) return;

            var rig = gameObject.GetComponent<Rigidbody>();
            rig.useGravity = false;
            rig.isKinematic = true;
            grenade.NetworkserverVelocities = new Grenades.RigidbodyVelocityPair
            {
                angular = Vector3.zero,
                linear = Vector3.zero
            };

            if (!PluginClass.C4Config.UpdatePosition) return;

            relativePosition = col.gameObject.transform.InverseTransformPoint(gameObject.transform.position);
            collider = col.gameObject;
        }

        public void Update()
        {
            if (collider != null)
                gameObject.transform.position = collider.transform.TransformPoint(relativePosition);
        }
    }
}
