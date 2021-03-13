using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float WalkSpeed;
    public float JumpForce;
    public float JetForce;
    public AnimationClip _walk, _jump;
    public Animation _Legs;
    public Transform _Blade, _GroundCast;
    public Camera cam;
    public MicInput micInput;
    public Texture2D texture;
    public Splatter splatter;
    public bool mirror;
    public float JetFuel;
    public float JetFuelConsumptionModifier = 1.0f;
    public float JetPowerModifier = 1.0f;
    public float InAirMod = 20.0f;
    
    public Vector2 respawnPos = new Vector2(5.9f, -40.9f);
    public int deathCount = 0;



    private bool _canJump, _canWalk, _canJet;
    private bool _isWalk, _isJump, _isJet;
    private float rot, _startScale;
    private Rigidbody2D rig;
    private Vector2 _inputAxis;
    private RaycastHit2D _hit;
    private float _jetFuelMax = 1.0f;
    

    void Start()
    {
        splatter.splatColor = new Color32(138,43,226,255);
        splatter.randomColor = false;
        rig = gameObject.GetComponent<Rigidbody2D>();
        _startScale = transform.localScale.x;
        JetFuel = _jetFuelMax;
    }

    void Update()
    {
        if (_hit = Physics2D.Linecast(new Vector2(_GroundCast.position.x, _GroundCast.position.y + 0.2f),
            _GroundCast.position))
        {
            if (!_hit.transform.CompareTag("Player"))
            {
                _canJump = true;
                _canWalk = true;
                JetFuel = _jetFuelMax;
            }
        }
        else _canJump = false;

        _inputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (_inputAxis.y > 0 && _canJump)
        {
            _canWalk = false;
            _isJump = true;
        }

        //Can jet if we have jumped
        _canJet = !_canJump;

        //Burns fuel while jetting
        if (_isJet)
        {
            JetFuel -= Time.deltaTime * JetFuelConsumptionModifier;
        }

        //Jets while pressing space
        //Change to microphone and change values of modifiers
        var micLoudness = micInput.getMicLoudness();
        if (micLoudness > 0.0001)
        {
            _isJet = _canJet;
            JetPowerModifier = micLoudness;
            JetFuelConsumptionModifier = micLoudness;
        }
        else
        {
            _isJet = false;
        }
    }

    void FixedUpdate()
    {
        Vector3 dir = cam.ScreenToWorldPoint(Input.mousePosition) - _Blade.transform.position;
        dir.Normalize();

        if (cam.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x + 0.2f)
            mirror = false;
        if (cam.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x - 0.2f)
            mirror = true;

        if (!mirror)
        {
            rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.localScale = new Vector3(_startScale, _startScale, 1);
            _Blade.transform.rotation = Quaternion.AngleAxis(rot, Vector3.forward);
        }

        if (mirror)
        {
            rot = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
            transform.localScale = new Vector3(-_startScale, _startScale, 1);
            _Blade.transform.rotation = Quaternion.AngleAxis(rot, Vector3.forward);
        }

        if (_inputAxis.x != 0)
        {
            if (_canJump)
                rig.velocity = new Vector2(_inputAxis.x * WalkSpeed * Time.deltaTime, rig.velocity.y);
            else
            {
                rig.AddForce(new Vector2(_inputAxis.x * InAirMod, 0));
            }

            if (_canWalk)
            {
                _Legs.clip = _walk;
                _Legs.Play();
            }
        }

        else if (_canJump)
        {
            rig.velocity = new Vector2(0, rig.velocity.y);
        }

        if (_isJump)
        {
            rig.AddForce(new Vector2(0, JumpForce));
            _Legs.clip = _jump;
            _Legs.Play();
            _canJump = false;
            _isJump = false;
        }

        //Jetpack force based on modifier
        if (_isJet && JetFuel >= 0)
        {
            rig.AddForce(new Vector2(0, JetForce * JetPowerModifier));
        }
    }

    public bool IsMirror()
    {
        return mirror;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, _GroundCast.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
                if (other.CompareTag("Surface"))
        {
            Splatter splatterObj = (Splatter)Instantiate(splatter, other.transform.position, Quaternion.identity);
                        handleDeath();
        }
    }

    private void handleDeath()
    {
        rig.position = respawnPos;
        deathCount++;

    }
    

}

