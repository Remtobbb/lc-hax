using UnityEngine;
using GameNetcodeStuff;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace Hax;

public class RigidbodyMovement : MonoBehaviour {
    const float baseSpeed = 25.0f;
    const float jumpForce = 12.0f;

    Rigidbody? rigidbody;
    SphereCollider? sphereCollider;

    float SprintMultiplier { get; set; } = 1.0f;
    List<Collider> CollidedColliders { get; } = [];

    public void Init() {
        if (Helper.LocalPlayer is not PlayerControllerB localPlayer) return;
        this.gameObject.layer = localPlayer.gameObject.layer;
    }

    void Awake() {
        this.rigidbody = this.gameObject.AddComponent<Rigidbody>();
        this.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        this.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        this.sphereCollider = this.gameObject.AddComponent<SphereCollider>();
        this.sphereCollider.radius = 0.25f;
    }

    void OnEnable() {
        if (this.rigidbody.Unfake() is not Rigidbody rigidbody) return;
        rigidbody.isKinematic = false;
    }

    void OnDisable() {
        if (this.rigidbody.Unfake() is not Rigidbody rigidbody) return;
        rigidbody.isKinematic = true;
    }

    void OnCollisionEnter(Collision collision) {
        this.CollidedColliders.Add(collision.collider);
    }

    void OnCollisionExit(Collision collision) {
        _ = this.CollidedColliders.Remove(collision.collider);
    }

    void Update() {
        Vector3 direction = new(
            Keyboard.current.dKey.ReadValue() - Keyboard.current.aKey.ReadValue(),
            Keyboard.current.spaceKey.ReadValue() - Keyboard.current.ctrlKey.ReadValue(),
            Keyboard.current.wKey.ReadValue() - Keyboard.current.sKey.ReadValue()
        );

        this.UpdateSprintMultiplier(Keyboard.current);
        this.Move(direction);

        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            this.Jump();
        }

        if (Keyboard.current.spaceKey.isPressed) {
            this.BunnyHop();
        }
    }

    void UpdateSprintMultiplier(Keyboard keyboard) {
        this.SprintMultiplier =
            keyboard.shiftKey.IsPressed()
                ? Mathf.Min(this.SprintMultiplier + (5.0f * Time.deltaTime), 5.0f)
                : 1.0f;
    }

    void Move(Vector3 direction) {
        if (this.rigidbody.Unfake() is not Rigidbody rigidbody) return;

        Vector3 forward = this.transform.forward;
        Vector3 right = this.transform.right;

        forward.y = 0.0f;
        right.y = 0.0f;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 translatedDirection = (right * direction.x) + (forward * direction.z);
        rigidbody.velocity += translatedDirection * Time.deltaTime * RigidbodyMovement.baseSpeed * this.SprintMultiplier;
    }

    void Jump() {
        if (this.rigidbody.Unfake() is not Rigidbody rigidbody) return;

        Vector3 newVelocity = rigidbody.velocity;
        newVelocity.y = RigidbodyMovement.jumpForce;
        rigidbody.velocity = newVelocity;
    }

    void BunnyHop() {
        if (this.CollidedColliders.Count <= 0) return;
        this.Jump();
    }
}
