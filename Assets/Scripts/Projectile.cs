using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  #region Variables
  public float speed;
  
  [Header("effect settings")]
  public GameObject muzzleEffectPrefab;
  public GameObject hitEffectPrefab;
  
  [Header("effect sound settings")]
  public AudioClip shotSFX;
  public AudioClip hitSFX;

  [HideInInspector] public AttackBehaviour attackBehaviour;
  [HideInInspector] public GameObject owner;
  [HideInInspector] public GameObject target;
  
  private bool isCollided;
  private Rigidbody rb;
  #endregion

  private void Start()
  {
    if (target != null)
    {
      Vector3 dest = target.transform.position;
      dest.y += 1.5f;
      transform.LookAt(dest);
    }
    
    // Ignore self collision
    if (owner)
    {
      Collider projectileCollider = GetComponent<Collider>();
      Collider[] ownerColliders = owner.GetComponentsInChildren<Collider>();
      foreach (Collider col in ownerColliders)
      {
        Physics.IgnoreCollision(projectileCollider, col);
      }
    }

    if (muzzleEffectPrefab != null)
    {
      GameObject muzzleVFX = Instantiate(muzzleEffectPrefab, transform.position, Quaternion.identity);
      muzzleVFX.transform.forward = gameObject.transform.forward;
      ParticleSystem particleSystem = muzzleVFX.GetComponent<ParticleSystem>();
      if (particleSystem)
      {
        Destroy(muzzleVFX, particleSystem.main.duration);
      }
      else
      {
        ParticleSystem childParticleSystem = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
        if (childParticleSystem)
          Destroy(muzzleVFX, childParticleSystem.main.duration);
      }
    }

    if (shotSFX != null && GetComponent<AudioSource>())
      GetComponent<AudioSource>().PlayOneShot(shotSFX);

    rb = GetComponent<Rigidbody>();
  }
  private void FixedUpdate()
  {
    if (speed != 0 && rb != null)
      rb.position += transform.forward * (speed * Time.deltaTime);
  }

  private void OnCollisionEnter(Collision other)
  { 
    if (isCollided) return;

    isCollided = true;
    Collider projectileCollider = GetComponent<Collider>();
    projectileCollider.enabled = false;
    
    if(hitSFX != null && GetComponent<AudioSource>())
      GetComponent<AudioSource>().PlayOneShot(hitSFX);

    speed = 0;
    rb.isKinematic = true;

    ContactPoint contactPoint = other.contacts[0];
    Quaternion contactRotation = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);
    Vector3 contactPosition = contactPoint.point;

    if (hitEffectPrefab)
    {
      GameObject hitVFX = Instantiate(hitEffectPrefab, contactPosition, contactRotation);
      ParticleSystem particleSystem = hitVFX.GetComponent<ParticleSystem>();
      if (particleSystem)
      {
        Destroy(hitVFX, particleSystem.main.duration);
      }
      else
      {
        ParticleSystem childParticleSystem = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
        if (childParticleSystem)
          Destroy(hitVFX, childParticleSystem.main.duration);
      }
    }

    IDamageable damaeable = other.gameObject.GetComponent<IDamageable>();
    if (damaeable != null)
    {
      int damage = attackBehaviour != null ? attackBehaviour.damage : 0;
      damaeable.TakeDamage(damage, null);
    }

    StartCoroutine(DestroyParticle(0.0f));
  }
  
  public IEnumerator DestroyParticle(float waitTime)
  {

    if (transform.childCount > 0 && waitTime != 0)
    {
      List<Transform> children = new List<Transform>();
      foreach (Transform t in transform.GetChild(0).transform)
      {
        children.Add(t);
      }

      while (transform.GetChild(0).localScale.x > 0)
      {
        yield return new WaitForSeconds(0.01f);
        transform.GetChild(0).localScale -= new Vector3(0.1f, 0.1f, 0.1f);
        for (int i = 0; i < children.Count; i++)
        {
          children[i].localScale -= new Vector3(0.1f, 0.1f, 0.1f);
        }
      }
    }

    yield return new WaitForSeconds(waitTime);
    Destroy(gameObject);
  }
}
