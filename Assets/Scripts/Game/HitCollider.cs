﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour {

    GameObject gameManager;
    GameObject gameCanvas;
    GameObject note;
    bool hitHeadNote;

    public Transform tapParticle;
    public Transform hitParticle;

    public GameObject perfectText;
    public GameObject greatText;
    public GameObject goodText;
    public GameObject badText;
    public GameObject missText;

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        gameCanvas = GameObject.Find("GameCanvas");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Note") || other.CompareTag("HeadNote") || other.CompareTag("TailNote"))
        {
            note = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Note") || other.CompareTag("TailNote"))
        {
            note = null;
        }

        else if (other.CompareTag("HeadNote"))
        {
            hitHeadNote = false;
            GameObject holdLane = note.transform.parent.transform.Find("HoldLane").gameObject;
            note = holdLane;
        }
    }

    public void OnPress()
    {
        FindObjectOfType<AudioManager>().Play("tap");
        Instantiate(tapParticle, transform.position, tapParticle.rotation);

        if (note == null)
        {
            return;
        }

        else if (note.CompareTag("Note") || note.CompareTag("TailNote"))
        {
            HandleNote(note.GetComponent<Note>().GetScoreType());
        }

        else if (note.CompareTag("HeadNote"))
        {
            hitHeadNote = true;
            GameObject holdLane = note.transform.parent.transform.Find("HoldLane").gameObject;
            HandleNote(note.GetComponent<Note>().GetScoreType());
            note = holdLane;
        }

        else if (note.CompareTag("HoldNote"))
        {
            DestroyHoldNote();
        }
    }

    public void OnRelease()
    {
        if (note == null)
        {
            return;
        }

        else if (note.CompareTag("TailNote") && hitHeadNote)
        {
            HandleNote(note.GetComponent<Note>().GetScoreType());
        }

        else if (note.CompareTag("HoldNote") && hitHeadNote)
        {
            DestroyHoldNote();
        }
    }

    public void OnHold()
    {
        if (note == null)
        {
            return;
        }

        else if (note.CompareTag("HoldNote"))
        {
            
        }
    }

    void DestroyHoldNote()
    {
        Destroy(note.transform.parent.gameObject);
        gameManager.GetComponent<GameManager>().ResetCombo();
        Instantiate(missText, new Vector3(gameCanvas.transform.position.x, gameCanvas.transform.position.y - 75.0f, gameCanvas.transform.position.z), Quaternion.identity, gameCanvas.transform);
        hitHeadNote = false;
    }

    void HandleNote(string type)
    {
        FindObjectOfType<AudioManager>().Play(type);
        Destroy(note);
        Instantiate(hitParticle, transform.position, hitParticle.rotation);

        if (type == "perfect")
        {
            Instantiate(perfectText, new Vector3(gameCanvas.transform.position.x, gameCanvas.transform.position.y - 75.0f, gameCanvas.transform.position.z), Quaternion.identity, gameCanvas.transform);
            gameManager.GetComponent<GameManager>().IncreaseCombo();
            gameManager.GetComponent<GameManager>().IncreaseScore(1000);
        }
        else if (type == "great")
        {
            Instantiate(greatText, new Vector3(gameCanvas.transform.position.x, gameCanvas.transform.position.y - 75.0f, gameCanvas.transform.position.z), Quaternion.identity, gameCanvas.transform);
            gameManager.GetComponent<GameManager>().IncreaseCombo();
            gameManager.GetComponent<GameManager>().IncreaseScore(750);
        }
        else if (type == "good")
        {
            Instantiate(goodText, new Vector3(gameCanvas.transform.position.x, gameCanvas.transform.position.y - 75.0f, gameCanvas.transform.position.z), Quaternion.identity, gameCanvas.transform);
            gameManager.GetComponent<GameManager>().ResetCombo();
            gameManager.GetComponent<GameManager>().IncreaseScore(500);
        }
        else if (type == "bad")
        {
            Instantiate(badText, new Vector3(gameCanvas.transform.position.x, gameCanvas.transform.position.y - 75.0f, gameCanvas.transform.position.z), Quaternion.identity, gameCanvas.transform);
            gameManager.GetComponent<GameManager>().ResetCombo();
            gameManager.GetComponent<GameManager>().IncreaseScore(250);
        }
    }
}
