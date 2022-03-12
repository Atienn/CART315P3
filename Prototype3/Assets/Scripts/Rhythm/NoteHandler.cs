using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteHandler : MonoBehaviour
{
    static Color buttonDefault = Color.white;
    static Color buttonHit = Color.black;

    [Header("General")]
    [SerializeField] AudioSource music;
    [SerializeField] int hitWindow;
    [SerializeField] int slowFactor;

    [SerializeField] GameObject[] notePrefabs = new GameObject[4];

    [Serializable]
    class RhythmButton {
        public Image arrow;
        public KeyCode key;
        public Vector2 noteDirection;
        public List<Note> notes;
        internal bool hit;
    }
    [SerializeField] RhythmButton[] buttons = new RhythmButton[4];

    // Start is called before the first frame update
    void Start()
    {
        for(byte i = 0; i < buttons.Length; i++) {
            
            Note last = null;
            foreach (Note note in buttons[i].notes) {
                if (last != null && note.time <= last.time) {
                    Debug.LogWarning($"Invalid timed notes ordering in button {i}.");
                }
                note.display = Instantiate(notePrefabs[i], buttons[i].arrow.transform).transform;
                last = note;
            }
        }
        DisplayNotes();
    }

    void Update() {
        for (byte i = 0; i < buttons.Length; i++) {
            buttons[i].hit = false;
        }

        DisplayNotes();

        #region Hit React

        for (byte i = 0; i < buttons.Length; i++) {
            if (Input.GetKeyDown(buttons[i].key)) {
                buttons[i].arrow.color = buttonHit;
            }
            else if (Input.GetKeyUp(buttons[i].key)) {
                buttons[i].arrow.color = buttonDefault;
            }
        }

        #endregion
    }

    void FixedUpdate() {
        foreach (RhythmButton btn in buttons) {
            //SHOULD PROBABLY HAVE A WAY TO DISABLE BUTTONS ONCE THEY HAVE NO MORE NOTES? (IS IT WORTH IT?)
            if (btn.notes.Count > 0) {
                if (btn.hit && Math.Abs(btn.notes[0].time - music.timeSamples) < hitWindow) {
                    Destroy(btn.notes[0].display.gameObject);
                    btn.notes.RemoveAt(0);
                }
                else if (btn.notes[0].time - music.timeSamples < 0) {
                    Destroy(btn.notes[0].display.gameObject);
                    btn.notes.RemoveAt(0);
                }
            }
        }
    }

    //Positions notes where they shoule be on screen.
    void DisplayNotes() {
        for (byte i = 0; i < buttons.Length; i++) {
            foreach (Note note in buttons[i].notes) {
                note.display.localPosition = buttons[i].noteDirection * ((note.time - music.timeSamples) / slowFactor);
            }
        }
    }
}
